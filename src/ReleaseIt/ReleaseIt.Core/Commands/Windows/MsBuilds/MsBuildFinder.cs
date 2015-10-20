using System;
using System.Collections.Generic;
using System.IO;

namespace ReleaseIt.Commands.Windows.MsBuilds
{
    public class MsBuildFinder : ProcessCommandFinder
    {
        public override string Name
        {
            get { return "BuildSetting"; }
        }

        public override string FindCmd()
        {
            var windir = Environment.ExpandEnvironmentVariables("%windir%");

            var dirPath = string.Format("{1}{0}Microsoft.NET{0}Framework", Path.AltDirectorySeparatorChar, windir);

            var dirs = new List<string>(Directory.GetDirectories(dirPath, "v*.*"));

            dirs.Sort((a, b) => string.Compare(b, a, StringComparison.Ordinal));

            return Path.Combine(dirs[0], "msbuild.exe");
        }
    }
}