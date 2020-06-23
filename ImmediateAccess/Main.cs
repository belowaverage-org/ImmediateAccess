using System.ServiceProcess;

namespace ImmediateAccess
{
    public partial class Main : ServiceBase
    {
        /// <summary>
        /// The service entry point.
        /// </summary>
        public Main()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Service on-start interface.
        /// </summary>
        protected override void OnStart(string[] args)
        {
            ImmediateAccess.Start(args);
        }
        /// <summary>
        /// Service on-stop interface.
        /// </summary>
        protected override void OnStop()
        {
            ImmediateAccess.Stop().Wait();
        }
    }
}