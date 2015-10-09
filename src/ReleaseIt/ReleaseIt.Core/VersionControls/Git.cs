using System;
using System.Collections.Generic;
using System.IO;
using ReleaseIt.CommandFinders;
using ReleaseIt.ParameterBuilder;

namespace ReleaseIt.VersionControls
{
    public class Git : VersionControl
    {
        private readonly ParameterWithValue<string> _branch = new ParameterWithValue<string>("branch")
        {
            ValueSplitChar = " ",
            Prefix = "--"
        };

        public Git()
            : base(new GitFinder())
        {
        }

        public override string Branch
        {
            get { return _branch.Value; }
            set { _branch.Value = value; }
        }

        protected override ICmdParameter[] BuildParameters(ExceuteResult executeResult)
        {
            string outputDir;
            if (IsClone(executeResult.ResultFolder, out outputDir))
            {
                return CloneParameters(outputDir);
            }
            return new ICmdParameter[]
            {
                new Parameter("", "pull")
            };
        }

        private ICmdParameter[] CloneParameters(string outputDir)
        {
            var result = new List<ICmdParameter>
            {
                new Parameter("", "clone"),
                new Parameter("", MakeUrl())
            };
            if (!string.IsNullOrEmpty(Branch))
            {
                result.Add(new ParameterWithValue<string>("branch")
                {
                    Value = Branch,
                    ValueSplitChar = " ",
                    HasSet = true,
                    Prefix = "--"
                });
            }
            result.Add(new Parameter("", outputDir));
            return result.ToArray();
        }

        protected override void UpdateExecuteResultInfo(ExceuteResult result)
        {
            result.
                ResultFolder = GetWorkingCopyName();

        }

        public string MakeUrl()
        {
            if (string.IsNullOrEmpty(UserName))
            {
                return Url;
            }
            var pos = Url.IndexOf("://", StringComparison.Ordinal);

            var prefix = Url.Substring(0, pos + 3);

            return prefix + UserName + ":" + Password + "@" + Url.Substring(pos + 3);
        }

#if DEBUG
        public
#else
            private 
#endif
 string GetWorkingCopyName()
        {
            if (WorkingCopy == "")
            {
                var pos = Url.LastIndexOf("/", StringComparison.Ordinal);
                var fileName = Url.Substring(pos + 1);
                return fileName.Split('.')[0];
            }
            return WorkingCopy;
            ;
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

        protected override void Invoke(ExceuteResult executeResult, string[] args, CommandSet commandSet)
        {
            string gitWorkingCopyPath = null;
            executeResult.ResultFolder = IsClone(executeResult.ResultFolder, out gitWorkingCopyPath)
                ? gitWorkingCopyPath
                : executeResult.ResultFolder;

            base.Invoke(executeResult, args, commandSet);
        }
    }
}