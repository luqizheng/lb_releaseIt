using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using ReleaseIt.ParameterBuilder;

namespace ReleaseIt
{
    public class MsBuild : Command
    {
        private readonly CmdParameter<LogLevel> _logLevel =
            new CmdParameter<LogLevel>("/v:", LogLevel.notSepcial, s => Convert.ToString(s).Substring(0,1));

        private readonly string _msBuildPath;


        public MsBuild(string msBuildPath):base(msBuildPath)
        {
            if (msBuildPath == null) throw new ArgumentNullException("msBuildPath");
            _msBuildPath = msBuildPath;
        }

        public bool IsEffect
        {
            get { return File.Exists(_msBuildPath); }
        }

        public LogLevel LogLevel
        {
            get { return _logLevel.Value; }
            set { _logLevel.Value = value; }
        }

        public void BuildTo(string solutionLocation, string configuration = "Release")
        {
            if (solutionLocation == null)
            {
                throw new ArgumentNullException("solutionLocation");
            }

            if (!File.Exists(solutionLocation))
            {
                throw new FileNotFoundException("File Not Found", solutionLocation);
            }

            var args = string.Format(CultureInfo.InvariantCulture,
                @"""{0}"" /target:Rebuild /p:Configuration=""{1}"" /fileLogger ",
                solutionLocation, //0
                configuration //1
                );

            args += this._logLevel.Build();

            var psi = new ProcessStartInfo(_msBuildPath, args)
            {
                UseShellExecute = false
            };
            using (var process = Process.Start(psi))
            {
                process.WaitForExit();
                if (process.ExitCode != 0)
                    throw new Exception(string.Format(CultureInfo.InvariantCulture, "{0} returned a non-zero exit code",
                        Path.GetFileName(psi.FileName)));
            }
        }
    }

    public enum LogLevel
    {
        notSepcial,
        quite,
        minimal,
        normal,
        detailed,
        diagnostic
    }
}