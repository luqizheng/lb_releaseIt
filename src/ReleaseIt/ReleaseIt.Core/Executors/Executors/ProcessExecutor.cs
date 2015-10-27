using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using ReleaseIt.Commands;

namespace ReleaseIt.Executors.Executors
{
    internal class ProcessExecutor : IExecutor
    {
        public void Invoke<T>(ICommand command, ExecuteSetting setting)
            where T : Setting
        {
            var cmd = command as ProcessCommand<T>;
            var commandPath = cmd.GetCommand(setting);
            var argus = cmd.BuildArguments(setting);
            var psi = new ProcessStartInfo(commandPath, argus)
            {
                UseShellExecute = false,
                WorkingDirectory = setting.WorkDirectory,
                CreateNoWindow = true,
                RedirectStandardInput = false,
                RedirectStandardOutput = true,
                RedirectStandardError = false
            };
            setting.Setting.ProcessLogger.Info(commandPath + " " + argus);
            using (var process = new Process())
            {
                process.EnableRaisingEvents = true;
                process.Exited += p_Exited;
                process.OutputDataReceived += (sender, e) => { command.OnOutput(e.Data, setting); };
                process.ErrorDataReceived += (sender, e) => { command.OnErrorOutput(e.Data, setting); };
                process.StartInfo = psi;

                process.Start();
                process.BeginOutputReadLine();
                process.Exited += (a, b) => { setting.Done = true; };
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

    public class CommandExecuteException
    {
        public CommandExecuteException()
        {

        }
    }
}