using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using ReleaseIt.ParameterBuilder;

namespace ReleaseIt.Commands.Windows.VersionControls
{
    [DataContract]
    public class Svn : ProcessCommand<VersionControlSetting>
    {
        public Svn(VersionControlSetting setting)
            : base(new SvnFinder())
        {
            Setting = setting;
        }

        protected override void InvokeByNewSetting(ExecuteSetting executeSetting, Setting setting)
        {
            base.InvokeByNewSetting(executeSetting, setting);
            executeSetting.WorkDirectory = null; //reset workDirectory
        }

        private ICmdParameter[] BuildParameters(ExecuteSetting executeResult)
        {
            var workingCopy = IoExtender.GetPath(executeResult.StartFolder, GetWorkingCopy());
            executeResult.ResultFolder = workingCopy;

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
                    list.Add(CreateParameter("username", Setting.UserName));
                    list.Add(CreateParameter("password", Setting.Password));
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

        private ParameterWithValue<string> CreateParameter(string key, string val)
        {
            return new ParameterWithValue<string>(key)
            {
                Prefix = "--",
                Value = val,
                ValueSplitChar = " "
            };
        }

        public override string BuildArguments(ExecuteSetting executoSetting)
        {
            var ary = BuildParameters(executoSetting).Select(s => s.Build());
            return string.Join(" ", ary);
        }
    }
}