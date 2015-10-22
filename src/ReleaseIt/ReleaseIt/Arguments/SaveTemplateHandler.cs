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
            commandSet.Svn("svn_sample")
                .Url("http://svn.address.com/trunk")
                .Auth("username", "password")
                .Tags("tag1","tab2")
                .WorkingCopy("workongfolder")
                .Tags("tag1", "tag2")
                ;

            commandSet
                .Build(true, "compileSample")
                .Dependency("svn_sample")
                .Tags("tag2", "tab3")
                .Release()
                .ProjectPath("/mypathfor.csproj")
                .CopyTo("publish/%prjName%");

            commandSet
                .CopyTo("publishFolder_or_network_path", "copySample")
                .Auth("networkPath_username", "networkPath_password")
                 .Dependency("compileSample")
                .Tags("tag4", "tab5")
                ;



            var manager = new SettingManager();
            manager.Save(commandSet, fileName, true);
            return false;
        }
    }
}