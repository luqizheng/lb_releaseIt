namespace ReleaseIt
{
    public class Exist : Setting
    {
        public override string Key
        {
            get { return "exit"; }
        }

        public override string Description
        {
            get { return "input 's' for save and exit or press any key to exit"; }
        }

        public override Setting Execute(CommandFactory commandFactory)
        {
            return null;
        }
    }
}