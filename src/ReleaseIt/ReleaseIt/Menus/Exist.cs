namespace ReleaseIt.Menus
{
    public class Exist : Menu
    {
        public override string Key
        {
            get { return "exit"; }
        }

        public override string Description
        {
            get { return "input 'y' for save and exit or press any key to exit"; }
        }

        public override Menu Do(CommandSet commandSet,string resultPath)
        {
            return null;
        }

        public Exist(string workingFolder) : base(workingFolder)
        {
        }
    }
}