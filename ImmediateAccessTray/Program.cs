using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace ImmediateAccessTray
{
    static class Program
    {
        public static string[] Arguments = new string[0];
        public static TrayIcon TrayIcon = null;
        public static TrayWindow TrayWindow = null;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            bool isDuplicateProcess = false;
            Process thisProc = Process.GetCurrentProcess();
            Process[] processes = Process.GetProcessesByName(thisProc.ProcessName);
            foreach (Process proc in processes)
            {
                if (proc.Id != thisProc.Id && proc.SessionId == thisProc.SessionId)
                {
                    isDuplicateProcess = true;
                    break;
                }
            }
            Arguments = args;
            if (Arguments.Contains("ForceTray")) isDuplicateProcess = false;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += Application_ThreadException;
            if (isDuplicateProcess)
            {
                TrayWindow = new TrayWindow();
                Application.Run(TrayWindow);
            }
            else
            {
                TrayIcon = new TrayIcon();
                Application.Run(TrayIcon);
            }
        }
        /// <summary>
        /// This event fires whenever the app context encounters an unexpected error.
        /// </summary>
        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            if (e.Exception.StackTrace != null)
            {
                MessageBox.Show(null, e.Exception.Message + "\r\n\r\n" + e.Exception.StackTrace, "Immediate Access", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show(null, e.Exception.Message, "Immediate Access", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}