using System.Collections;
using System.ComponentModel;
using System.ServiceProcess;

namespace ImmediateAccess
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }
        protected override void OnAfterInstall(IDictionary savedState)
        {
            ServiceController sc = new ServiceController("ImmediateAccess");
            sc.Start();
            sc.Dispose();
            base.OnAfterInstall(savedState);
        }
    }
}