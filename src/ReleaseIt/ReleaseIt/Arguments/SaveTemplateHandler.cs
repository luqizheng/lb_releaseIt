using System.IO;
using ReleaseIt.WindowCommand.Publish;

namespace ReleaseIt.Arguments
{
    public class SaveTemplateHandler : ArgumentHandler
    {
        public SaveTemplateHandler()
        {
           
        }

        public override string Key
        {
            get { return "t"; }
        }

        public override bool Handle(CommandSet set, string fileName, string argument)
        {
            var command = new CommandSet(new FileInfo(fileName).Directory.FullName);
            command.ForWidnow();

            command.Svn()
                .Url("http://svn.address.com/trunk")
                .Auth("username", "password")
                .WorkingCopy("workongfolder")
                .Name("Svn_name_for_target");

            command
                .Build(true).Release().ProjectPath("/mypathfor.csproj")
                .CopyTo("publish/%prjName%")
                .Name("BuildName")
                ;

            command.CopyTo("publish").Name("");
            return false;
        }
    }
}