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
    public abstract class Command
    {
        protected Command(ICommandFinder finder)
        {
            Finder = finder;
        }
        [JsonIgnore]
        public ICommandFinder Finder { get; set; }


        protected abstract ICmdParameter[] BuildParameters(string executeFolder);

        protected abstract ExceuteResult CreateResult(string executeFolder);


        public ExceuteResult Invoke(string executeFolder)
        {
            var arguments = BuildParameters(executeFolder)
                .Select(item => item.Build()).Where(arg => !string.IsNullOrEmpty(arg)).ToArray();
            Invoke(executeFolder, arguments);
            return CreateResult(executeFolder);
        }

        private string GetExecuteCommandPath()
        {
            var path = Finder.FindCmd();
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("command file not found.", path);
            }
            return path;
        }

        protected virtual void Invoke(string workingDirectory, string[] args)
        {
            var commandPath = GetExecuteCommandPath();

            var psi = new ProcessStartInfo(commandPath, string.Join(" ", args))
            {
                UseShellExecute = false,
                WorkingDirectory = workingDirectory,
                CreateNoWindow = true,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
            };
            Console.WriteLine(commandPath + " " + string.Join(" ", args));
            using (var process = new Process())
            {

                process.EnableRaisingEvents = true;
                process.Exited += new EventHandler(p_Exited);
                process.OutputDataReceived += new DataReceivedEventHandler(p_OutputDataReceived);
                process.ErrorDataReceived += new DataReceivedEventHandler(p_ErrorDataReceived);
                process.StartInfo = psi;
                process.Start();
                process.WaitForExit();
                if (process.ExitCode != 0)
                    throw new Exception(string.Format(CultureInfo.InvariantCulture, "{0} returned a non-zero exit code",
                        Path.GetFileName(psi.FileName)));
            }
        }
        void p_OutputDataReceived(Object sender, DataReceivedEventArgs e)
        {
            //这里是正常的输出
            Console.WriteLine(e.Data);

        }

        void p_ErrorDataReceived(Object sender, DataReceivedEventArgs e)
        {
            //这里得到的是错误信息
            Console.WriteLine(e.Data);

        }

        void p_Exited(Object sender, EventArgs e)
        {
            Console.WriteLine("finish");
        }

    }

    public class ExceuteResult
    {
        public string ResultPath { get; set; }
    }
}