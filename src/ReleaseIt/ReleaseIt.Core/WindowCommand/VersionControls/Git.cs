using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ReleaseIt.ParameterBuilder;
using ReleaseIt.WindowCommand.CommandFinders;

namespace ReleaseIt.WindowCommand.VersionControls
{
    public class Git : Command
    {


        public Git(VersionControlSetting setting)
            : base(new GitFinder())
        {
            Setting = setting;
        }

        public VersionControlSetting Setting { get; set; }

        private ICmdParameter[] CloneParameters(string outputDir)
        {
            var result = new List<ICmdParameter>
            {
                new Parameter("", "clone"),
                new Parameter("", MakeUrl())
            };
            if (!string.IsNullOrEmpty(Setting.Branch))
            {
                result.Add(new ParameterWithValue<string>("branch")
                {
                    Value = Setting.Branch,
                    ValueSplitChar = " ",
                    Prefix = "--"
                });
            }
            result.Add(new Parameter("", outputDir));
            return result.ToArray();
        }

        public string MakeUrl()
        {
            if (string.IsNullOrEmpty(Setting.UserName))
            {
                return Setting.Url;
            }
            var pos = Setting.Url.IndexOf("://", StringComparison.Ordinal);

            var prefix = Setting.Url.Substring(0, pos + 3);

            return prefix + Setting.UserName + ":" + Setting.Password + "@" + Setting.Url.Substring(pos + 3);
        }

#if DEBUG
        public
#else
            private 
#endif
 string GetWorkingCopyName()
        {
            if (Setting.WorkingCopy == "")
            {
                //var pos = Setting.Url.LastIndexOf("/", StringComparison.Ordinal);
                return Path.GetFileNameWithoutExtension(Setting.Url);
            }
            return Setting.WorkingCopy;
        }

        /// <summary>
        ///     是否使用Clone 还是Pull
        /// </summary>
        /// <param name="executeFolder"></param>
        /// <returns></returns>
        private bool IsClone(string executeFolder, out string workingFolder)
        {
            workingFolder = Path.GetFullPath(executeFolder + GetWorkingCopyName());
            return !Directory.Exists(workingFolder);
        }

        public override string BuildArguments(ExecuteSetting executoSetting)
        {
            ICmdParameter[] paramsCmdParameters;
            string outputDir;

            if (IsClone(executoSetting.StartFolder, out outputDir))
            {
                paramsCmdParameters = CloneParameters(outputDir);
            }
            else
            {
                paramsCmdParameters = new ICmdParameter[]
                {
                    new Parameter("", "pull")
                };
            }
            executoSetting.WorkDirectory = outputDir;
            var ary = paramsCmdParameters.Select(s => s.Build());
            return string.Join(" ", ary);
        }
    }
}