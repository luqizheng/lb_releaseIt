using ReleaseIt.Menus.SetupMenu;

namespace ReleaseIt.Menus
{
    public class BuildNew : MenusList
    {
        public BuildNew()

        {
            Add(new VersionControlCommandMenu());
            Add(new MsBuildCommandMenu());
        }

        public override string Key
        {
            get { return "n"; }
        }

        public override string Description
        {
            get { return "Build a new Command factory."; }
        }
    }
}