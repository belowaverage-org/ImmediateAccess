using System.ServiceProcess;

namespace ImmediateAccess
{
    public partial class Main : ServiceBase
    {
        public Main()
        {
            InitializeComponent();
        }
        protected override void OnStart(string[] args)
        {
            ImmediateAccess.Start(args).Wait();
        }
        protected override void OnStop()
        {
            ImmediateAccess.Stop().Wait();
        }
    }
}
