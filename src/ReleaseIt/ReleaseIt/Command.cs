using System;
using System.Collections.Generic;
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
        public ICommandFinder Finder { get; set; }


        protected Command(ICommandFinder finder)
        {
            Finder = finder;

        }


        protected abstract ICmdParameter[] BuildParameters(string executeFolder);

        protected abstract ExceuteResult CreateResult(string executeFolder);


        public ExceuteResult Invoke(string executeFolder)
        {
            Invoke(BuildParameters(executeFolder)
                .Select(item => item.Build()).Where(arg => !string.IsNullOrEmpty(arg)).ToArray());
            return CreateResult(executeFolder);
        }

        private string GetExecuteCommandPath()
        {
            var path = this.Finder.FindCmd();
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("command file not found.", path);
            }
            return path;
        }

        private void Invoke(string[] args)
        {
            var commandPath = GetExecuteCommandPath();

            var psi = new ProcessStartInfo(commandPath, string.Join(" ", args))
            {
                UseShellExecute = true,
                ErrorDialog = true,
            };
            using (var writer = new StreamWriter("commandLog.txt"))
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