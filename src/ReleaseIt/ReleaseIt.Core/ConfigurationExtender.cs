using ReleaseIt.Commands.Windows.Executors;
using ReleaseIt.Commands.Windows.MsBuilds;
using ReleaseIt.Commands.Windows.Publish;
using ReleaseIt.Commands.Windows.VersionControls;
using ReleaseIt.WindowCommand;

namespace ReleaseIt
{
    public static class ConfigurationExtender
    {
        public static ExecuteSetting ForWidnow(this ExecuteSetting executSetting)
        {
            var commandSettings = new ConfigurationSetting();
            commandSettings.Regist(typeof(VersionControlSetting), setting =>
            {
                var vcSetting = (VersionControlSetting)setting;
                if (vcSetting.VersionControlType == VersionControlType.Git)
                    return new Git(vcSetting);
                return new Svn(vcSetting);
            });

            commandSettings.Executor = new ProcessExecutor();
            commandSettings.Regist(typeof(BuildSetting), setting => new MsBuildCommand((BuildSetting)setting));
            commandSettings.Regist(typeof(CopySetting), setting => new XCopy((CopySetting)setting));
            executSetting.Setting = commandSettings;
            return executSetting;
        }
    }
}