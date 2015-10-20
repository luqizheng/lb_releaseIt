using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using ReleaseIt.Executors;

namespace ReleaseIt.Commands.Windows.Executors
{
    internal class ProcessExecutor : IExecutor
    {
        public void Invoke<T>(ProcessCommand<T> command, ExecuteSetting setting)
            where T : Setting
        {
            var commandPath = command.GetCommand(setting);
            var argus = command.BuildArguments(setting);
            var psi = new ProcessStartInfo(commandPath, argus)
            {
                UseShellExecute = false,
                WorkingDirectory = setting.WorkDirectory,
                CreateNoWindow = true,
                RedirectStandardInput = false,
                RedirectStandardOutput = true,
                RedirectStandardError = false
            };
            Console.WriteLine(commandPath + " " + argus);
            using (var process = new Process())
            {
                process.EnableRaisingEvents = true;
                process.Exited += p_Exited;
                process.OutputDataReceived += (sender, e) =>
                {
                    command.OnOutput(e.Data);
                };
                process.ErrorDataReceived += (sender, e) =>
                {
                    command.OnErrorOutput(e.Data);
                };
                process.StartInfo = psi;

                process.Start();
                process.BeginOutputReadLine();
                process.WaitForExit();
                if (process.ExitCode != 0)
                    throw new Exception(string.Format(CultureInfo.InvariantCulture,
                        "{0} returned a non-zero exit code",
                        Path.GetFileName(psi.FileName)));
            }
        }



        private void p_Exited(object sender, EventArgs e)
        {
           
        }
    }
}