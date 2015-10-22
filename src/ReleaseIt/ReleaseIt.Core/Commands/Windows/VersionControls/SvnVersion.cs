using System.IO;
using System.Text.RegularExpressions;

namespace ReleaseIt.Commands.Windows.VersionControls
{
    public class SvnVersion : ProcessCommand<VersionControlSetting>
    {
        public SvnVersion(VersionControlSetting setting)
            : base(new SvnFinder(), setting)
        {
        }

        protected override void InvokeByNewSetting(ExecuteSetting executeSetting, Setting setting)
        {
            base.InvokeByNewSetting(executeSetting, setting);
            using (var reader = new StreamReader("version.txt"))
            {
                var s = Regex.Replace(reader.ReadToEnd(), "\\S", "");
                executeSetting.AddVariable("%version%", s);
            }
        }


        public override string BuildArguments(ExecuteSetting executoSetting)
        {
            return "info " + executoSetting.ResultFolder + " |find \"Revision\" > version.txt";
        }
    }
}