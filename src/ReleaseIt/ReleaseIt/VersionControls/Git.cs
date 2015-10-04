using System.Collections.Generic;
using System.IO;
using ReleaseIt.CommandFinders;
using ReleaseIt.ParameterBuilder;

namespace ReleaseIt.VersionControls
{
    public class Git : Command
    {
        private readonly ParameterWithValue<string> _branch = new ParameterWithValue<string>("branch")
        {
            ValueSplitChar = " ",
            Prefix = "--"
        };

        private readonly Parameter _workingCopy = new Parameter("", "");

        public Git()
            : base(new GitFinder())
        {
        }

        public string WorkingCopy
        {
            get { return _workingCopy.Name; }
            set { _workingCopy.Name = value; }
        }

        public string Url { get; set; }

        public string Branch
        {
            get { return _branch.Value; }
            set { _branch.Value = value; }
        }

        public string UserName { get; set; }

        public string Password { get; set; }

        protected override ICmdParameter[] BuildParameters(string executeFolder)
        {

            if (IsClone(executeFolder))
            {
                return CloneParameters(executeFolder);
            }
            return new ICmdParameter[]
            {
                new Parameter("", "pull")
            };
        }

        private ICmdParameter[] CloneParameters(string executeFolder)
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
            result.Add(new Parameter("", GetWorkingCopyName()));
            return result.ToArray();
        }

        protected override ExceuteResult CreateResult(string executeFolder)
        {
            return new ExceuteResult
            {
                ResultPath = executeFolder
            };
        }

        public string MakeUrl()
        {
            if (string.IsNullOrEmpty(UserName))
            {
                return Url;
            }
            var pos = Url.IndexOf("://");

            var prefix = Url.Substring(0, pos + 3);

            return prefix + UserName + ":" + Password + "@" + Url.Substring(pos + 3);
        }

        public string GetWorkingCopyName()
        {
            if (WorkingCopy == "")
            {
                var pos = Url.LastIndexOf("/");
                var fileName = Url.Substring(pos+1);
                return fileName.Split('.')[0];
            }
            return WorkingCopy;
            ;

        }
        /// <summary>
        /// 是否使用Clone 还是Pull
        /// </summary>
        /// <param name="executeFolder"></param>
        /// <returns></returns>
        private bool IsClone(string executeFolder)
        {
            var workingFolder = Path.Combine(executeFolder, GetWorkingCopyName());
            return !Directory.Exists(workingFolder);
        }
        protected override void Invoke(string workingDirectory, string[] args)
        {
            var gitWorkingCopyPath = Path.Combine(workingDirectory, this.GetWorkingCopyName());
            base.Invoke(!Directory.Exists(gitWorkingCopyPath) ? workingDirectory : gitWorkingCopyPath, args);
        }
    }
}