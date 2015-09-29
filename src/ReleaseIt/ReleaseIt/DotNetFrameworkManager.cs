using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ReleaseIt
{
    public class DotNetFrameworkManager
    {
        public IList<DotNetFramework> GetVersion()
        {
            var windir = Environment.ExpandEnvironmentVariables("%windir%");

            var dirPath = string.Format("{1}{0}Microsoft.NET{0}Framework", Path.AltDirectorySeparatorChar, windir);

            var dirs = Directory.GetDirectories(dirPath, "v*.*");
            return dirs.Select(dir => Create(new DirectoryInfo(dir))).ToList();
        }

        private DotNetFramework Create(DirectoryInfo dir)
        {
            var result = new DotNetFramework
            {
                Version = dir.Name,
                FullPath = dir.FullName
            };
            return result;
        }
    }
}