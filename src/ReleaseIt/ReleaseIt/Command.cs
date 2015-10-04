using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
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
                ErrorDialog = false,
                WorkingDirectory = workingDirectory
            };
            using (var writer = new StreamWriter("commandLog.txt", append: true))
            {
                writer.WriteLine(commandPath + " " + string.Join(" ", args));
            }
            using (var process = Process.Start(psi))
            {
                process.WaitForExit(1000 * 60);
                if (process.ExitCode != 0)
                    throw new Exception(string.Format(CultureInfo.InvariantCulture, "{0} returned a non-zero exit code",
                        Path.GetFileName(psi.FileName)));
            }
        }
    }

    public class ExceuteResult
    {
        public string ResultPath { get; set; }
    }
}