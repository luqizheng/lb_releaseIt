using ReleaseIt.IniStore;

namespace ReleaseIt.Arguments
{
    public class SaveTemplateHandler : ArgumentHandler
    {
        public override string Key
        {
            get { return "t"; }
        }

        public override bool Handle(CommandSet commandSet, string fileName, string argument)
        {
            commandSet.Svn()
                .Url("http://svn.address.com/trunk")
                .Auth("username", "password")
                .WorkingCopy("workongfolder")
                .Name("Svn_name_for_target");

            commandSet
                .Build(true).Release().ProjectPath("/mypathfor.csproj")
                .CopyTo("publish/%prjName%")
                .Name("BuildName")
                ;

            commandSet.CopyTo("publish").Name("");

            var manager = new SettingManager();
            manager.Save(commandSet, fileName);
            return false;
        }
    }
}