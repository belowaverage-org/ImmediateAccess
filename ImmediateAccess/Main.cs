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
            ImmediateAccess.Start(args);
        }
        protected override void OnStop()
        {
            ImmediateAccess.Stop();
        }
    }
}
