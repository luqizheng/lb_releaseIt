using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using ReleaseIt.ParameterBuilder;
using ReleaseIt.WindowCommand.CommandFinders;

namespace ReleaseIt.WindowCommand.VersionControls
{
    [DataContract]
    public class Svn : Command<VersionControlSetting>
    {
        public Svn(VersionControlSetting setting)
            : base(new SvnFinder())
        {
            Setting = setting;
        }

        public override VersionControlSetting Setting { get; protected set; }

        private ICmdParameter[] BuildParameters(ExecuteSetting executeResult)
        {
            var workingCopy = IoExtender.GetPath(executeResult.StartFolder, GetWorkingCopy());

            if (!Directory.Exists(workingCopy))
            {
                var list = new List<ICmdParameter>
                {
                    new Parameter("", "checkout"),
                    new Parameter("", Setting.Url),
                    new Parameter("", workingCopy)
                };
                if (Setting.UserName != null)
                {
                    list.Add(new ParameterWithValue<string>("username")
                    {
                        Prefix = "--",
                        Value = Setting.UserName,
                        ValueSplitChar = " "
                    });
                    list.Add(new ParameterWithValue<string>("password")
                    {
                        Prefix = "--",
                        Value = Setting.Password,
                        ValueSplitChar = " "
                    });
                }
                return list.ToArray();
            }
            executeResult.WorkDirectory = workingCopy; //update需要改变workDirecotry.
            return new ICmdParameter[]
            {
                new Parameter("", "update")
            };
        }

        private string GetWorkingCopy()
        {
            if (string.IsNullOrEmpty(Setting.WorkingCopy))
            {
                return Path.GetFileNameWithoutExtension(Setting.Url);
            }
            return Setting.WorkingCopy;
        }


        public override string BuildArguments(ExecuteSetting executoSetting)
        {
            var ary = BuildParameters(executoSetting).Select(s => s.Build());
            return string.Join(" ", ary);
        }
    }
}