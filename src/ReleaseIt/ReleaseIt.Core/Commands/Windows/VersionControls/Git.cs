using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using ReleaseIt.ParameterBuilder;
using ReleaseIt.WindowCommand.CommandFinders;

namespace ReleaseIt.Commands.Windows.VersionControls
{
    [DataContract]
    public class Git : ProcessCommand<VersionControlSetting>
    {
        public const string FolderNameVariableName = "%gitName%";

        public Git(VersionControlSetting setting)
            : base(new GitFinder(),setting)
        {
            
        }

      

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
            if (outputDir.Contains(" "))
            {
                outputDir = string.Format("\"{0}\"", outputDir);
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
            if (!string.IsNullOrEmpty(Setting.WorkingCopy) && Setting.WorkingCopy.Contains(FolderNameVariableName))
            {
                return Setting.WorkingCopy.Replace(FolderNameVariableName, Path.GetFileNameWithoutExtension(Setting.Url));
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
            workingFolder = IoExtender.GetPath(executeFolder, GetWorkingCopyName());
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