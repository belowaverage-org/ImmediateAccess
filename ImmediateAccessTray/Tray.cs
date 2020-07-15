using ImmediateAccessTray.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;
using System.ServiceProcess;

namespace ImmediateAccessTray
{
    public partial class Tray : Form
    {
        private ServiceController IAS = null;
        public Tray()
        {
            InitializeComponent();
            Icon = Resources.Icon;
        }
        private void lblWebsite_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(lblWebsite.Text);
        }
        private async void btnToggleService_Click(object sender, EventArgs e)
        {
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
        }
        private void Tray_Load(object sender, EventArgs e)
        {
            IAS = new ServiceController("ImmediateAccess");
            Assembly selfAssem = Assembly.GetExecutingAssembly();
            lblVersion.Text = selfAssem.GetCustomAttribute<AssemblyFileVersionAttribute>().Version;
            lblAuthor.Text = selfAssem.GetCustomAttribute<AssemblyCompanyAttribute>().Company;
            lblWebsite.Text = selfAssem.GetCustomAttribute<AssemblyMetadataAttribute>().Value;
            tbDescription.Text = selfAssem.GetCustomAttribute<AssemblyDescriptionAttribute>().Description;
            RefreshServiceStatus();
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
            if (lblServiceStatus.InvokeRequired)
            {
                lblServiceStatus.Invoke(DelegateRefreshServiceStatus);
            }
            else
            {
                DelegateRefreshServiceStatus.Invoke();
            }
        }
    }
}
