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
                {
                    var defau = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Run " + command.ToString() + " fail");
                    Console.WriteLine(commandPath + " " + argus);
                    Console.ForegroundColor = defau;
                    throw new CommandRunningException(argus, commandPath, cmd.Id);
                }
            }
        }


        private void p_Exited(object sender, EventArgs e)
        {
        }
    }

    public class CommandRunningException : Exception
    {
        public string ComandId { get; set; }
        private readonly string _arg;
        private readonly string _cmd;

        public CommandRunningException(string arg, string cmd, string comandId)
        {
            this.ComandId = comandId;
            _arg = arg;
            _cmd = cmd;
        }
        public string Command { get { return _cmd; } }
        public string Arguments { get { return _arg; } }
        public override string Message
        {
            get { return "Invoke Command returned a non-zero exit code"; }
        }
    }
    public class CommandExecuteException
    {
        public CommandExecuteException()
        {

        }
    }
}