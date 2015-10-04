﻿using ReleaseIt.MsBuilds;
using ReleaseIt.VersionControls;

namespace ReleaseIt
{
    public static class ConfigurationExtender
    {
        public static MsBuildBuilder MsBuildForWeb(this CommandFactory factory)
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
            factory.Add(msbuid);
            return new MsBuildBuilder(msbuid, true);
        }

        public static SvnBuilder Svn(this CommandFactory factory)
        {
            var d = new Svn();
            factory.Add(d);
            return new SvnBuilder(d);
        }

        public static GitBuilder Git(this CommandFactory factory)
        {
            var d = new Git();
            factory.Add(d);
            return new GitBuilder(d);
        }
    }
}