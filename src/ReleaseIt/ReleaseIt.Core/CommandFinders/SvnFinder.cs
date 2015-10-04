using System.Collections.Generic;
using System.IO;

namespace ReleaseIt.CommandFinders
{
    public class SvnFinder : ICommandFinder
    {
        public string Name
        {
            get { return "Svn"; }
        }

        public string FindCmd()
        {
            var list = new List<string>();
            foreach (string driver in System.Environment.GetLogicalDrives())
            {
                list.Add(Path.Combine(driver, "Program Files", "VisualSVN", "bin", "svn.exe"));
                if (System.Environment.Is64BitOperatingSystem)
                {
                    list.Add(Path.Combine(driver, "Program Files (x86)", "VisualSVN", "bin", "svn.exe"));
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