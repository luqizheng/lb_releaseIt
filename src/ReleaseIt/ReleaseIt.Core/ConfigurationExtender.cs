using ReleaseIt.Commands;
using ReleaseIt.Commands.Windows.MsBuilds;
using ReleaseIt.Commands.Windows.Publish;
using ReleaseIt.Commands.Windows.VersionControls;
using ReleaseIt.Executors.Executors;
using ReleaseIt.IniStore;

namespace ReleaseIt
{
    public static class ConfigurationExtender
    {
        public static ExecuteSetting ForWidnow(this ExecuteSetting executSetting)
        {
            CommonRegist();
            CommandSettingMap.Regist(typeof (GitSetting), setting =>
            {
                var vcSetting = (GitSetting) setting;
                return new GitCommand(vcSetting);
            });

            CommandSettingMap.Regist(typeof (SvnSetting), setting =>
            {
                var vcSetting = (SvnSetting) setting;
                return new SvnCommand(vcSetting);
            });
            CommandSettingMap.Regist(typeof (CompileSetting), setting => new MsBuildCommand((CompileSetting) setting));
            CommandSettingMap.Regist(typeof (CopySetting), setting => new XCopyCommand((CopySetting) setting));

            var commandSettings = new ConfigurationSetting {Executor = new ProcessExecutor()};
            executSetting.Setting = commandSettings;
            return executSetting;
        }

        private static void CommonRegist()
        {
            CommandSettingMap.Regist(typeof (SmtpEmailSetting), setting => new EmailCommand((SmtpEmailSetting) setting));
        }
    }
}