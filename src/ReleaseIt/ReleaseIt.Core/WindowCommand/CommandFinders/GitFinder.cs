using System;
using System.Collections.Generic;
using System.IO;

namespace ReleaseIt.WindowCommand.CommandFinders
{
    public class GitFinder : ICommandFinder
    {
        public string Name
        {
            get { return "Git"; }
        }

        public string FindCmd()
        {
            var list = new List<string>();
            foreach (var driver in Environment.GetLogicalDrives())
            {
                list.Add(Path.Combine(driver, "Program Files"+ "Git", "bin", "Git.exe"));
                if (Environment.Is64BitOperatingSystem)
                {
                    list.Add(Path.Combine(driver, "Program Files (x86)", "Git", "bin", "Git.exe"));
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