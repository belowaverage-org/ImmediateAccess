using ImmediateAccessTray.Properties;
using ImmediateAccess;
using System;
using System.Drawing;
using System.Reflection;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;
using System.ServiceProcess;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Threading;
using System.Security.Principal;

namespace ImmediateAccessTray
{
    public partial class Tray : Form
    {
        private MemoryMappedFile mConsole;
        private MemoryMappedViewStream mConsoleStream;
        private StreamReader mConsoleReader;
        private ServiceController IAS;
        private byte[] mConsoleTimestamp;
        private byte[] mConsoleTimestampCompare;
        private CancellationTokenSource conReadLoopCTS;
        public Tray()
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
                psi.FileName = Application.ExecutablePath;
                psi.Verb = "RunAs";
                Process.Start(psi);
                Close();
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
            RefreshAllStatus();
        }
        private void SetupMConsole()
        {
            Task.Run(() => {
                try
                {
                    mConsole = MemoryMappedFile.OpenExisting("ImmediateAccessConsole");
                    mConsoleStream = mConsole.CreateViewStream();
                    mConsoleReader = new StreamReader(mConsoleStream);
                    mConsoleTimestamp = new byte[16];
                    mConsoleTimestampCompare = new byte[16];
                    conReadLoopCTS = new CancellationTokenSource();
                    _ = mConsoleReadLoop();
                }
                catch (IOException)
                {
                    Invoke(new Action(() => {
                        MessageBox.Show(this, "The Immediate Access service may not be running, there was no virtual console present to read from.", "Error reading logs.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }));
                }
            });
        }
        private void RefreshAllStatus()
        {
            RefreshServiceStatus();
            RefreshPolicyStatus();
        }
        private void RefreshPolicyStatus()
        {
            PolicyReader.ReadPolicies();
            if (PolicyReader.IsServiceEnabled())
            {
                lblServicePolicy.ForeColor = Color.DarkGreen;
                lblServicePolicy.Text = "Active";
            }
            else
            {
                lblServicePolicy.ForeColor = Color.Red;
                lblServicePolicy.Text = "De-Activated";
            }
        }
        private Task mConsoleReadLoop()
        {
            return Task.Run(() => {
                while(true)
                {
                    Thread.Sleep(100);
                    if (conReadLoopCTS.IsCancellationRequested) break;
                    if (mConsoleReader == null) continue;
                    mConsoleStream.Position = 0;
                    mConsoleStream.Read(mConsoleTimestamp, 0, 16);
                    if (mConsoleTimestamp.ByteCompare128(mConsoleTimestampCompare)) continue;
                    mConsoleTimestampCompare = (byte[])mConsoleTimestamp.Clone();
                    mConsoleStream.Position = 16;
                    UpdateLogRTF(mConsoleReader.ReadToEnd());
                }
            });
        }
        private void UpdateLogRTF(string log)
        {
            Invoke(new Action(() => {
                rtbLogs.Text = log;
                rtbLogs.Select(rtbLogs.Text.Length, 0);
                rtbLogs.ScrollToCaret();
            }));
        }
        private Action DelegateRefreshServiceStatus = new Action(() =>
        {
            ServiceController IAS = Program.Tray.IAS;
            Label lblServiceStatus = Program.Tray.lblServiceStatus;
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
                if (conReadLoopCTS != null) conReadLoopCTS.Cancel();
            }
        }
        private bool IsElevated()
        {
            return WindowsIdentity.GetCurrent().Groups.Contains(new SecurityIdentifier(WellKnownSidType.BuiltinAdministratorsSid, null));
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
    }
}