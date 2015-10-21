using ReleaseIt.IniStore;

namespace ReleaseIt.Arguments
{
    public class SaveTemplateHandler : ArgumentHandler
    {
        public override string Key
        {
            get { return "c"; }
        }

        public override bool Handle(CommandSet commandSet, string fileName, string argument)
        {
            commandSet.Svn()
                .Url("http://svn.address.com/trunk")
                .Auth("username", "password")
                .WorkingCopy("workongfolder")
                .Name("Svn_name_for_target").Tags("tag1", "tag2");

            commandSet
                .Build(true)
                .Release()
                .ProjectPath("/mypathfor.csproj")
                .CopyTo("publish/%prjName%")
                .Name("BuildName")
                ;

            commandSet
                .CopyTo("publishFolder_or_network_path")
                .Auth("networkPath_username", "networkPath_password")
                .Name("copy_command_name");


            var manager = new SettingManager();
            manager.Save(commandSet, fileName, true);
            return false;
        }
    }
}