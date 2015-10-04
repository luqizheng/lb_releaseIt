using System;
using System.Windows.Forms;
using ReleaseIt;

namespace Release
{
    internal static class Program
    {
        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main(string[] arguments)
        {
            if (arguments == null || arguments.Length == 0)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
            }
            else
            {
                Console.WriteLine("LB_Release to run.");
                Console.WriteLine("Current Directory:Environment.CurrentDirectory");
                var fileName = "";
                var parameter = "";
                foreach (var arg in arguments)
                {
                    if (arg.StartsWith("-"))
                    {
                        parameter = arg;
                        fileName = arg.Split(':')[1];
                    }
                }

                CommandFactory.CreateFrom(fileName).Invoke(Environment.CurrentDirectory);
            }
        }
    }
}