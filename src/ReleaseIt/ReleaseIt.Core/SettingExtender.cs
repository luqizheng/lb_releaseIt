using System.Runtime.CompilerServices;
using ReleaseIt.Commands;
using ReleaseIt.Commands.Windows.MsBuilds;
using ReleaseIt.Commands.Windows.Publish;
using ReleaseIt.Commands.Windows.VersionControls;
using ReleaseIt.SettingBuilders;

namespace ReleaseIt
{
    public static class SettingExtender
    {
        public static CopySettingBuilder CopyTo(this CommandSet set, string path, string id)
        {
            var setting = new CopySetting { Id = id };
            set.Add(new XCopyCommand(setting));
            setting.TargetPath = path;
            return new CopySettingBuilder(setting);
        }

        public static CompileSettingBuilder Build(this CommandSet set, bool isWeb, string id)
        {
            var buildSetting = new CompileSetting
            {
                IsWeb = isWeb,
                Id = id,
            };
            set.Add(new MsBuildCommand(buildSetting));
            return new CompileSettingBuilder(buildSetting);
        }


        public static VcBuilder Svn(this CommandSet set, string id)
        {
            var result = new SvnSetting()
            {
              
                Id = id,
            };
            set.Add(new SvnCommand(result));
            return new VcBuilder(result);
        }

        public static VcBuilder Git(this CommandSet set, string id)
        {
            var setting = new GitSetting()
            {
               
                Id = id,
            };
            set.Add(new GitCommand(setting));
            return new VcBuilder(setting);
        }

        public static SmtpEmailBuilder SmtpEmail(this CommandSet set, string id)
        {
            var setting = new SmtpEmailSetting {Id = id};

            var command = new EmailCommand(setting);

            set.Add(command);
            var result = new SmtpEmailBuilder(setting);
            return result;

        }
    }
}