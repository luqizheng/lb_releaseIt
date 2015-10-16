using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;

namespace ReleaseIt.WindowCommand
{
    internal class ProcessExecutor
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
            Console.WriteLine("ProcessCommand execute complete.");
        }
    }
}