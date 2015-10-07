using ReleaseIt.Settings;

namespace ReleaseIt
{
    public class BuildNew : Menus
    {
        public BuildNew()

        {
            Add(new VersionControlSetting());
            Add(new MsBuildSetting());
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