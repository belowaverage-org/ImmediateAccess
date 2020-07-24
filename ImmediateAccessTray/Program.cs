using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using System.Windows.Forms;

namespace ImmediateAccessTray
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Arguments = args;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += Application_ThreadException;
            TrayIcon = new TrayIcon();
            Application.Run(TrayIcon);
        }
        public static string[] Arguments = new string[0];
        public static TrayIcon TrayIcon = null;
        public static TrayWindow TrayWindow = null;
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
