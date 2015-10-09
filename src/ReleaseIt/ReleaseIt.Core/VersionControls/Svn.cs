using System;
using System.IO;
using ReleaseIt.CommandFinders;
using ReleaseIt.ParameterBuilder;

namespace ReleaseIt.VersionControls
{
    public class Svn : VersionControl
    {
        public Svn()
            : base(new SvnFinder())
        {
        }

        private string _preWorkingPath;
        public override string Branch { get; set; }


        protected override ICmdParameter[] BuildParameters(ExceuteResult executeResult)
        {
            var workingCopy = IoExtender.GetPath(executeResult.StartFolder, WorkingCopy);

            if (!Directory.Exists(workingCopy))
            {
                return new ICmdParameter[]
                {
                    new Parameter("", "checkout")
                    {
                        HasSet = true
                    },
                    new Parameter("", Url)
                    {
                        HasSet = true
                    },
                    new Parameter("", workingCopy),
                    new ParameterWithValue<string>("username")
                    {
                        Prefix = "--",
                        Value = UserName,
                        ValueSplitChar = " "
                    },
                    new ParameterWithValue<string>("password")
                    {
                        Prefix = "--",
                        Value = Password,
                        ValueSplitChar = " "
                    }
                };
            }
            else
            {
                _preWorkingPath = executeResult.WorkDirectory;
                executeResult.WorkDirectory = workingCopy; //update需要改变workDirecotry.
                return new ICmdParameter[]
                {
                    new Parameter("", "update"),
                };
            }
        }

        protected override void UpdateExecuteResultInfo(ExceuteResult result)
        {
            result.ResultFolder = Path.GetFullPath(result.ResultFolder + WorkingCopy);
            if (_preWorkingPath != null)
            {
                result.WorkDirectory = _preWorkingPath;
                _preWorkingPath = null;
            }

        }
    }
}