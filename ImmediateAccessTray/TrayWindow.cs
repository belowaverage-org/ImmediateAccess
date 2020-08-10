using ImmediateAccessTray.Properties;
using ImmediateAccess;
using System;
using System.Drawing;
using System.Reflection;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;
using System.ServiceProcess;
using System.Security.Principal;
using System.Linq;
using System.Net.NetworkInformation;
using PInvoke;
using System.IO;
using System.Threading;

namespace ImmediateAccessTray
{
    public partial class TrayWindow : Form
    {
        private Process nConsoleProc;
        private ServiceController IAS;
        private bool CurrentlyUpdatingStatuses = false;
        public TrayWindow()
        {
            InitializeComponent();
            Icon = Resources.Icon;
            if (!IsElevated()) pbShieldIcon.Image = new Icon(SystemIcons.Shield, 16, 16).ToBitmap();
        }
        private void lblWebsite_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(lblWebsite.Text);
        }
        private async void btnToggleService_Click(object sender, EventArgs e)
        {
            if (!IsElevated())
            {
                RunAsAdmin();
                return;
            }
            btnToggleService.Enabled = false;
            await Task.Run(() => {
                RefreshServiceStatus();
                if (IAS.Status == ServiceControllerStatus.Running)
                {
                    IAS.Stop();
                    RefreshServiceStatus();
                    IAS.WaitForStatus(ServiceControllerStatus.Stopped);
                }
                else
                {
                    IAS.Start();
                    RefreshServiceStatus();
                    IAS.WaitForStatus(ServiceControllerStatus.Running);
                }
                RefreshServiceStatus();
            });
            btnToggleService.Enabled = true;
        }
        private void RunAsAdmin()
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo();
                psi.Arguments = Program.Arguments.ToSpaceDelimitedString() + " ElevatedStartStopService";
                psi.FileName = Application.ExecutablePath;
                psi.Verb = "RunAs";
                Process.Start(psi);
                Program.TrayIcon.ExitThread();
            }
            catch (Exception)
            {
                DialogResult result = MessageBox.Show(this, "Cannot start or stop the Immediate Access service, access was denied.", "User Account Control", MessageBoxButtons.RetryCancel, MessageBoxIcon.Information);
                if (result == DialogResult.Retry) RunAsAdmin();
            }
        }
        private void Tray_Load(object sender, EventArgs e)
        {
            IAS = new ServiceController("ImmediateAccess");
            Assembly selfAssem = Assembly.GetExecutingAssembly();
            lblVersion.Text = selfAssem.GetCustomAttribute<AssemblyFileVersionAttribute>().Version;
            lblAuthor.Text = selfAssem.GetCustomAttribute<AssemblyCompanyAttribute>().Company;
            lblWebsite.Text = selfAssem.GetCustomAttribute<AssemblyMetadataAttribute>().Value;
            tbDescription.Text = selfAssem.GetCustomAttribute<AssemblyDescriptionAttribute>().Description;
            NetworkChange.NetworkAddressChanged += NetworkChange_NetworkAddressChanged;
            RefreshAllStatus();
            if (Program.Arguments.Contains("ElevatedStartStopService")) btnToggleService_Click(null, null);
        }
        private void NetworkChange_NetworkAddressChanged(object sender, EventArgs e)
        {
            RefreshAllStatus();
        }
        private void RefreshAllStatus()
        {
            Task.Run(() => {
                if (CurrentlyUpdatingStatuses) return;
                CurrentlyUpdatingStatuses = true;
                Invoke(new Action(() => {
                    lblNetLocation.ForeColor =
                    lblNetStatus.ForeColor =
                    lblVpnStatus.ForeColor =
                    lblServicePolicy.ForeColor =
                    lblServiceStatus.ForeColor =
                    Color.Orange;
                    lblNetLocation.Text =
                    lblNetStatus.Text =
                    lblVpnStatus.Text =
                    lblServicePolicy.Text =
                    lblServiceStatus.Text =
                    "Refreshing...";
                }));
                RefreshServiceStatus();
                if (RefreshPolicyStatus())
                {
                    RefreshVPNStatus();
                    RefreshNetworkStatus();
                    RefreshNetworkLocation();
                }
                else
                {
                    Invoke(new Action(() => {
                        lblVpnStatus.Text =
                        lblNetStatus.Text =
                        lblNetLocation.Text =
                        "Unknown";
                    }));
                }
                CurrentlyUpdatingStatuses = false;
            });
        }
        private async void RefreshNetworkLocation()
        {
            bool result = await TestNetwork.IsProbeAvailable();
            Invoke(new Action(() =>
            {
                lblNetLocation.ForeColor = Color.Black;
                if (result)
                {
                    lblNetLocation.Text = "Internal";
                }
                else
                {
                    lblNetLocation.Text = "External";
                }
            }));
        }
        private async void RefreshNetworkStatus()
        {
            bool result = await TestNetwork.IsProbeAvailable(true);
            Invoke(new Action(() =>
            {
                if (result)
                {
                    lblNetStatus.ForeColor = Color.DarkGreen;
                    lblNetStatus.Text = "Connected";
                }
                else
                {
                    lblNetStatus.ForeColor = Color.DarkRed;
                    lblNetStatus.Text = "Not Connected";
                }
            }));
        }
        private async void RefreshVPNStatus()
        {
            string profile = await VpnControl.IsConnected();
            Invoke(new Action(() =>
            {
                lblVpnStatus.ForeColor = Color.Black;
                if (profile == null)
                {
                    lblVpnStatus.Text = "Not Connected";
                }
                else
                {
                    lblVpnStatus.Text = "Connected";
                }
            }));
        }
        private bool RefreshPolicyStatus()
        {
            PolicyReader.ReadPolicies();
            bool result = PolicyReader.IsServiceEnabled();
            Invoke(new Action(() => {
                if (result)
                {
                    lblServicePolicy.ForeColor = Color.DarkGreen;
                    lblServicePolicy.Text = "Active";
                }
                else
                {
                    lblServicePolicy.ForeColor = Color.Red;
                    lblServicePolicy.Text = "De-Activated";
                }
            }));
            return result;
        }
        private Action DelegateRefreshServiceStatus = new Action(() =>
        {
            Label lblServiceStatus = Program.TrayWindow.lblServiceStatus;
            try
            {
                ServiceController IAS = Program.TrayWindow.IAS;
                IAS.Refresh();
                if (IAS.Status == ServiceControllerStatus.Running)
                {
                    lblServiceStatus.ForeColor = Color.DarkGreen;
                    lblServiceStatus.Text = "Running";
                }
                if (IAS.Status == ServiceControllerStatus.Stopped)
                {
                    lblServiceStatus.ForeColor = Color.Red;
                    lblServiceStatus.Text = "Stopped";
                }
                if (IAS.Status == ServiceControllerStatus.StartPending)
                {
                    lblServiceStatus.ForeColor = Color.DarkOrange;
                    lblServiceStatus.Text = "Starting...";
                }
                if (IAS.Status == ServiceControllerStatus.StopPending)
                {
                    lblServiceStatus.ForeColor = Color.DarkOrange;
                    lblServiceStatus.Text = "Stopping...";
                }
            }
            catch (Exception)
            {
                lblServiceStatus.ForeColor = Color.Red;
                lblServiceStatus.Text = "Not Installed";
            }
        });
        private void RefreshServiceStatus()
        {
            if (InvokeRequired)
            {
                Invoke(DelegateRefreshServiceStatus);
            }
            else
            {
                DelegateRefreshServiceStatus.Invoke();
            }
        }
        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab == tpLogs)
            {
                SetupMConsole();
            }
            else
            {
                if (nConsoleProc != null && !nConsoleProc.HasExited) nConsoleProc.Kill();
            }
        }
        private void SetupMConsole()
        {
            tpLogs.Hide();
            Process proc = Process.GetCurrentProcess();
            string name = proc.MainModule.ModuleName;
            string path = proc.MainModule.FileName.Replace(name, "");
            string nConPath = Path.Combine(path, "ImmediateAccessNConsole.exe");
            nConsoleProc = new Process();
            nConsoleProc.StartInfo = new ProcessStartInfo()
            {
                FileName = nConPath,
                WindowStyle = ProcessWindowStyle.Minimized
            };
            nConsoleProc.Start(); // PROCESS NEEDS TO MONITOR PARENT OR SOMETHING 
            while (nConsoleProc.MainWindowHandle.ToInt32() == 0x0) Thread.Sleep(10);
            User32.SetParent(nConsoleProc.MainWindowHandle, tpLogs.Handle);
            User32.SetWindowLong(nConsoleProc.MainWindowHandle, User32.WindowLongIndexFlags.GWL_STYLE, User32.SetWindowLongFlags.WS_VISIBLE);
            User32.SetWindowPos(nConsoleProc.MainWindowHandle, IntPtr.Zero, 0, 0, tpLogs.Width, tpLogs.Height, User32.SetWindowPosFlags.SWP_NOACTIVATE);
            tpLogs.Show();
        }
        private bool IsElevated()
        {
            return WindowsIdentity.GetCurrent().Groups.Contains(new SecurityIdentifier(WellKnownSidType.BuiltinAdministratorsSid, null));
        }
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshAllStatus();
        }
        private void pbLogo_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            Application.Exit();
        }
        private void tpLogs_Resize(object sender, EventArgs e)
        {
            if (nConsoleProc != null && !nConsoleProc.HasExited)
            {
                User32.SetWindowPos(nConsoleProc.MainWindowHandle, IntPtr.Zero, 0, 0, tpLogs.Width, tpLogs.Height,
                    User32.SetWindowPosFlags.SWP_NOACTIVATE |
                    User32.SetWindowPosFlags.SWP_NOMOVE
                );
            }
        }
        private void TrayWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (nConsoleProc != null && !nConsoleProc.HasExited) nConsoleProc.Kill();
        }
    }
    public static class Extensions
    {
        public static bool ByteCompare128(this byte[] self, byte[] compare)
        {
            bool equal = true;
            for (int i = 0; i < 16; i++)
            {
                if (self[i] != compare[i])
                {
                    equal = false;
                    break;
                }
            }
            return equal;
        }
        public static string ToSpaceDelimitedString(this string[] self)
        {
            string result = "";
            foreach (string element in self)
            {
                result += " " + element;
            }
            return result;
        }
    }
}