using System.IO;

namespace ReleaseIt
{
    public class DotNetFramework
    {
        internal DotNetFramework()
        {
        }

        public string Version { get; internal set; }

        public MsBuild MsBuild
        {
            get
            {
                var msbuild = new MsBuild(Path.Combine(FullPath, "msbuild.exe"));
                return msbuild;
            }
        }

        public string FullPath { get; internal set; }
    }
}