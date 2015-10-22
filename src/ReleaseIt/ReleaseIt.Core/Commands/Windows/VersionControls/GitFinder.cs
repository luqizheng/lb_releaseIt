using System;
using System.Collections.Generic;
using System.IO;

namespace ReleaseIt.Commands.Windows.VersionControls
{
    public class GitFinder : ProcessCommandFinder
    {
        public override string Name
        {
            get { return "GitCommand"; }
        }

        public override string FindCmd()
        {
            var list = new List<string>();
            foreach (var driver in Environment.GetLogicalDrives())
            {
                list.Add(Path.Combine(driver, "Program Files" + "GitCommand", "bin", "GitCommand.exe"));
                if (Environment.Is64BitOperatingSystem)
                {
                    list.Add(Path.Combine(driver, "Program Files (x86)", "GitCommand", "bin", "GitCommand.exe"));
                }
            }

            foreach (var path in list)
            {
                if (File.Exists(path))
                    return path;
            }
            return "";
        }
    }
}