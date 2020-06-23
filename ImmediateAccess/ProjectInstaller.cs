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
        /// <summary>
        /// This method starts the service after being installed.
        /// </summary>
        protected override void OnAfterInstall(IDictionary savedState)
        {
            ServiceController sc = new ServiceController("ImmediateAccess");
            sc.Start();
            sc.Dispose();
            base.OnAfterInstall(savedState);
        }
    }
}