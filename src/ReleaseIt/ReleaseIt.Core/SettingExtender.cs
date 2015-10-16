using ReleaseIt.SettingBuilders;
using ReleaseIt.WindowCommand.Publish;

namespace ReleaseIt
{
    public static class SettingExtender
    {
        public static CopySettingBuilder CopyTo(this CommandSet set, string path)
        {
            var r = new CopySetting();
            set.Add(r);
            r.TargetPath = path;
            return new CopySettingBuilder(r);
        }

        public static BuildSettingBuilder Build(this CommandSet set, bool isWeb)
        {
            var buildSetting = new BuildSetting
            {
                IsWeb = isWeb
            };
            set.Add(buildSetting);
            return new BuildSettingBuilder(buildSetting);
        }


        public static VCBuilder Svn(this CommandSet set)
        {
            var result = new VersionControlSetting
            {
                VersionControlType = VersionControlType.Svn
            };
            set.Add(result);
            return new VCBuilder(result);
        }

        public static VCBuilder Git(this CommandSet set)
        {
            var result = new VersionControlSetting
            {
                VersionControlType = VersionControlType.Git
            };
            set.Add(result);
            return new VCBuilder(result);
        }
    }
}