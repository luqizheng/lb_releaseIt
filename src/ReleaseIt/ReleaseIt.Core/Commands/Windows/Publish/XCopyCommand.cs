using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using ReleaseIt.ParameterBuilder;

namespace ReleaseIt.Commands.Windows.Publish
{
    [DataContract]
    public class XCopyCommand : ProcessCommand<CopySetting>
    {
        public XCopyCommand(CopySetting setting)
            : base(new DefaultCommand("xcopy", "xcopy"), setting)
        {

        }

        public XCopyCommand()
            : base(new DefaultCommand("xcopy", "xcopy"), new CopySetting())
        {
        }


        private Parameter[] BuildParameters(ExecuteSetting executeResult)
        {
            var result = new List<Parameter>
            {
                CreateFromPath(executeResult),
                GetTargetPath(executeResult),
                new Parameter("y"),
                new Parameter("s"),
                new Parameter("r"),
                new Parameter("i"),
                //new Parameter("f")
            };
            if (Setting.LastCopyDate != null && Setting.UseDateCompareCopy)
            {
                result.Add(new ParameterWithValue<string>("d", s => Setting.LastCopyDate)
                {
                    Value = Setting.LastCopyDate
                });
            }
            if (Setting.UseDateCompareCopy)
            {
                SettingChanged = true;
                Setting.LastCopyDate = DateTime.Now.ToString("MM-dd-yyyy");
            }
            executeResult.ResultFolder = result[1].Name; //copy 的目录
            return result.ToArray();
        }

        private Parameter CreateFromPath(ExecuteSetting executeSetting)
        {
            var srcPath = executeSetting.BuildByVariable(Setting.SourcePath);
            var result = new Parameter("", IoExtender.WrapperPath(srcPath));
            return result;
        }

        private Parameter GetTargetPath(ExecuteSetting result)
        {

            var targetPath = IoExtender.GetPath(result.ResultFolder, result.BuildByVariable(Setting.TargetPath));
            if (targetPath.StartsWith("\\\\") && Setting.UserName != null)
            {
                targetPath = @"\\" + Setting.UserName + ":" + Setting.Password + "@" + targetPath.Substring(2);
            }


            var directory = new DirectoryInfo(targetPath);
            if (!directory.Exists)
            {
                directory.CreateEx();
            }
            targetPath = result.BuildByVariable(targetPath, true);
            var finalPath = new Parameter("", targetPath);
            return finalPath;
        }


        public override string BuildArguments(ExecuteSetting executoSetting)
        {
            var arguments = string.Join(" ", BuildParameters(executoSetting).Select(s => s.Build()));
            return arguments;
        }

        public override ICommand Clone()
        {
            return new XCopyCommand(Setting.Clone() as CopySetting);
        }
    }
}