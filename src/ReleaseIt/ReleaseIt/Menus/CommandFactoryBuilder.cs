using ReleaseIt.Menus.SetupMenu;

namespace ReleaseIt.Menus
{
    public class CommandFactoryBuilder : MenusList
    {
        private readonly bool _isNew;

        public CommandFactoryBuilder(bool isNew, string workFolder) : base(workFolder)
        {
            _isNew = isNew;
            Add(new VersionControlCommandMenu(workFolder));
            Add(new MsBuildCommandMenu(workFolder));
        }

        public override string Key
        {
            get { return "n"; }
        }

        public override string Description
        {
            get { return _isNew ? "Create new Command Set." : "Add new Command for this Command Set."; }
        }
    }
}