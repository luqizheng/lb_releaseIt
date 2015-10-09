using System;
using System.IO;
using System.Linq;

namespace ReleaseIt.MsBuilds
{
    public class MsBuildCommandBuilder
    {
        private readonly bool _isWebProj;
        private readonly MsBuild _msBuild;
        internal string OutDir = "outDir";
        internal string WebProjectOutputDir = "WebProjectOutputDir";

        public MsBuildCommandBuilder(MsBuild msBuild, bool isWebProj)
        {
            if (msBuild == null) throw new ArgumentNullException("msBuild");
            _msBuild = msBuild;
            _isWebProj = isWebProj;
        }

        public MsBuildCommandBuilder ProjectPath(string path)
        {
            _msBuild.ProjectPath = path;
            return this;
        }


        public MsBuildCommandBuilder CopyTo(string path)
        {
            var fileInfo = new FileInfo(_msBuild.ProjectPath);
            string folderName = fileInfo.Name;
            if (fileInfo.Extension != "")
            {
                folderName = fileInfo.Name.Replace(fileInfo.Extension, fileInfo.Name);
            }

            if (!path.EndsWith(folderName))
            {
                path = path +"/"+ folderName;
            }
            _msBuild.AddProperty(_isWebProj ? "WebProjectOutputDir" : "outDir", path);
            _msBuild.Target =
                _msBuild.Target.Concat(new[] {"_CopyWebApplication", "_WPPCopyWebApplication", "TransformWebConfig"})
                    .ToArray();
            return this;
        }

        public MsBuildCommandBuilder Release()
        {
            _msBuild.AddProperty("Configuration", "Release");
            return this;
        }

        public MsBuildCommandBuilder Debug()
        {
            _msBuild.AddProperty("Configuration", "Debug");
            return this;
        }

        /// <summary>
        ///     编译为 debug / release 或者其他设置
        /// </summary>
        /// <param name="setting"></param>
        /// <returns></returns>
        public MsBuildCommandBuilder BuildBy(string setting)
        {
            _msBuild.AddProperty("Configuration", "setting");
            return this;
        }
    }
}