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
using System.Net.Sockets;
using System.IO;
using System.Collections.Generic;

namespace ImmediateAccessTray
{
    public partial class TrayWindow : Form
    {
        private TcpClient nConsole;
        private StreamReader nConsoleReader;
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
        private void SetupMConsole()
        {
            rtbLogs.Text = "";
            Task.Run(() => {
                try
                {
                    nConsole = new TcpClient("127.0.0.1", 7362);
                    nConsoleReader = new StreamReader(nConsole.GetStream());
                    _ = mConsoleReadLoop();
                }
                catch (Exception)
                {
                    Invoke(new Action(() => {
                        MessageBox.Show(this, "Cannot connect to the Immediate Access net console, the service might not be running.", "Immediate Access", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }));
                }
            });
        }
        private Task mConsoleReadLoop()
        {
            return Task.Run(() => {
                while (true)
                {
                    try
                    {
                        UpdateLogRTF(nConsoleReader.ReadLine() + "\r\n");
                    }
                    catch (Exception)
                    {
                        return;
                    }
                }
            });
        }
        private void UpdateLogRTF(string log)
        {
            Color color = Color.White;
            string[] logParts = log.Split(new string[] { "<", ">" }, StringSplitOptions.None);
            foreach (string logPart in logParts)
            {
                if (ConsoleColorConversion.ContainsKey(logPart))
                {
                    color = ConsoleColorConversion[logPart];
                }
                else
                {
                    UpdateLogColor(logPart, color);
                }
            }
        }
        private void UpdateLogColor(string log, Color color)
        {
            Invoke(new Action(() => {
                rtbLogs.SelectionColor = color;
                rtbLogs.AppendText(log);
                rtbLogs.Select(rtbLogs.Text.Length, 0);
                rtbLogs.ScrollToCaret();
            }));
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
                if (nConsoleReader != null) nConsoleReader.Close();
            }
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
        private Dictionary<string, Color> ConsoleColorConversion = new Dictionary<string, Color>()
        {
            { "#Black#", Color.Black },
            { "#Blue#", Color.Blue },
            { "#Cyan#", Color.Cyan },
            { "#DarkBlue#", Color.DarkBlue },
            { "#DarkCyan#", Color.DarkCyan },
            { "#DarkGray#", Color.DarkGray },
            { "#DarkGreen#", Color.DarkGreen },
            { "#DarkMagenta#", Color.DarkMagenta },
            { "#DarkRed#", Color.DarkRed },
            { "#DarkYellow#", Color.Gold },
            { "#Gray#", Color.Gray },
            { "#Green#", Color.Green },
            { "#Magenta#", Color.Magenta },
            { "#Red#", Color.Red },
            { "#White#", Color.White },
            { "#Yellow#", Color.Yellow }
        };
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