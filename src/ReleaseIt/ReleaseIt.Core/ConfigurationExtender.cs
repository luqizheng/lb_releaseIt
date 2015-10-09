using ReleaseIt.MsBuilds;
using ReleaseIt.Publish;
using ReleaseIt.VersionControls;

namespace ReleaseIt
{
    public static class ConfigurationExtender
    {
        public static MsBuildCommandBuilder MsBuild(this CommandSet set, bool forWeb)
        {
            var msbuid = new MsBuild
            {
                BuildLogFile = true,
                Target = new[]
                {
                    "ResolveReferences",
                    "Compile" 
                }
            };
            msbuid.AddProperty("_ResolveReferenceDependencies", "true");
            set.Commands.Add(msbuid);
            return new MsBuildCommandBuilder(msbuid, forWeb);
        }

        public static VersionControlerBuilder Svn(this CommandSet set)
        {
            var d = new Svn();
            set.Commands.Add(d);
            return new VersionControlerBuilder(d);
        }

        public static VersionControlerBuilder Git(this CommandSet set)
        {
            var d = new Git();
            set.Commands.Add(d);
            return new VersionControlerBuilder(d);
        }

        public static VersionControlerBuilder Git(this CommandSet set,string url)
        {
            var d = new Git();
            set.Commands.Add(d);
            return new VersionControlerBuilder(d);
        }


        public static XCopyBuilder CopyTo(this CommandSet set, string path)
        {
            var d = new XCopy()
            {
                TargetPath = path
            };
            set.Commands.Add(d);
            return new XCopyBuilder(d);
        }


    }
}