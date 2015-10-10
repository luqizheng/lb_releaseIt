using System.IO;
using System.Linq;
using ReleaseIt.ParameterBuilder;
using ReleaseIt.WindowCommand.CommandFinders;

namespace ReleaseIt.WindowCommand.VersionControls
{
    public class Svn : Command
    {
        

        public Svn(VersionControlSetting setting)
            : base(new SvnFinder())
        {
            Setting = setting;
        }

        public VersionControlSetting Setting { get; set; }

        private ICmdParameter[] BuildParameters(ExecuteSetting executeResult)
        {
            var workingCopy = IoExtender.GetPath(executeResult.StartFolder, Setting.WorkingCopy);

            if (!Directory.Exists(workingCopy))
            {
                return new ICmdParameter[]
                {
                    new Parameter("", "checkout"),
                    new Parameter("", Setting.Url),
                    new Parameter("", workingCopy),
                    new ParameterWithValue<string>("username")
                    {
                        Prefix = "--",
                        Value = Setting.UserName,
                        ValueSplitChar = " "
                    },
                    new ParameterWithValue<string>("password")
                    {
                        Prefix = "--",
                        Value = Setting.Password,
                        ValueSplitChar = " "
                    }
                };
            }
            executeResult.WorkDirectory = workingCopy; //update需要改变workDirecotry.
            return new ICmdParameter[]
            {
                new Parameter("", "update")
            };
        }


        public override string BuildArguments(ExecuteSetting executoSetting)
        {
            var ary = BuildParameters(executoSetting).Select(s => s.Build());
            return string.Join(" ", ary);
        }
    }
}