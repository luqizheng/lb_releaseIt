using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using ReleaseIt.CommandFinders;
using ReleaseIt.ParameterBuilder;

namespace ReleaseIt
{
    public abstract class Command : ICommand
    {
        protected Command(ICommandFinder finder)
        {
            Finder = finder;
        }

        [JsonIgnore]
        public ICommandFinder Finder { get; set; }


        protected abstract ICmdParameter[] BuildParameters(ExceuteResult executeResult);

        protected virtual void UpdateExecuteResultInfo(ExceuteResult result)
        {
        }


        public ExceuteResult Invoke(ExceuteResult executeResult, CommandSet commandSet)
        {
            var arguments = BuildParameters(executeResult)
                .Select(item => item.Build()).Where(arg => !string.IsNullOrEmpty(arg)).ToArray();
            Invoke(executeResult, arguments, commandSet);
            UpdateExecuteResultInfo(executeResult);
            return executeResult;
            ;
        }

        private string GetExecuteCommandPath()
        {
            var path = Finder.FindCmd();
            //if (!File.Exists(path))
            //{
            //    throw new FileNotFoundException("command file not found.", path);
            //}
            return path;
        }

        protected virtual void BeforeInvoke(ProcessStartInfo startInfo)
        {
            
        }
        protected virtual void Invoke(ExceuteResult executeResult, string[] args, CommandSet commandSet)
        {
            var commandPath = GetExecuteCommandPath();

            var psi = new ProcessStartInfo(commandPath, string.Join(" ", args))
            {

                UseShellExecute = false,
                WorkingDirectory = executeResult.WorkDirectory,
                CreateNoWindow = true,
                RedirectStandardInput = false,
                RedirectStandardOutput = true,
                RedirectStandardError = false,
            };
            BeforeInvoke(psi);
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

        public override string ToString()
        {
            return Finder.Name;
        }
    }
}