using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using ReleaseIt.Executor;

namespace ReleaseIt.WindowCommand
{
    public class ProcessExecutor : IExecutor
    {
        public void Invoke(ICommand command, ExecuteSetting setting)
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

            using (var process = new Process())
            {
                process.EnableRaisingEvents = true;
                process.Exited += p_Exited;
                process.OutputDataReceived += p_OutputDataReceived;
                process.ErrorDataReceived += p_ErrorDataReceived;
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

        private void p_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            //这里是正常的输出
            Console.WriteLine(e.Data);
        }

        private void p_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            //这里得到的是错误信息
            Console.WriteLine(e.Data);
        }

        private void p_Exited(object sender, EventArgs e)
        {
            Console.WriteLine("Command execute complete.");
        }
    }
}