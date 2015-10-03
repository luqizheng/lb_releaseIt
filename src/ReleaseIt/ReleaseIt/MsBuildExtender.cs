using System;
using System.Linq;
using ReleaseIt.MsBuilds;

namespace ReleaseIt
{
    public class MsBuildBuilder
    {
        private readonly MsBuild _msBuild;
        private readonly bool _isWebProj;
        internal string WebProjectOutputDir = "WebProjectOutputDir";
        internal string OutDir = "outDir";
        public MsBuildBuilder(MsBuild msBuild, bool isWebProj)
        {
            _msBuild = msBuild;
            _isWebProj = isWebProj;
        }

        public MsBuildBuilder ProjectPath(string path)
        {
            _msBuild.ProjectPath = path;
            return this;
        }


        public MsBuildBuilder CopyTo(string path)
        {
            _msBuild.AddProperty(_isWebProj ? "WebProjectOutputDir" : "outDir", path);
            _msBuild.Target = _msBuild.Target.Concat(new[] { "_CopyWebApplication", "_WPPCopyWebApplication" }).ToArray();
            return this;
        }

        public MsBuildBuilder Release()
        {
            _msBuild.AddProperty("Configuration","Release");
            return this;
        }

        public MsBuildBuilder Debug()
        {
            _msBuild.AddProperty("Configuration", "Debug");
            return this;
        }

        /// <summary>
        /// 编译为 debug / release 或者其他设置
        /// </summary>
        /// <param name="setting"></param>
        /// <returns></returns>
        public MsBuildBuilder BuildBy(string setting)
        {
            _msBuild.AddProperty("Configuration", "setting");
            return this;
        }
    }

    public static class MsBuildExtender
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
    }
}