namespace ReleaseIt
{
    public class Top : Menus
    {
        public Top()
            : base()
        {
            Add(new SetOrder());
            Add(new BuildNew());
            
        }

        public override string Key
        {
            get { return "top"; }
        }

        public override string Description
        {
            get { return "top"; }
        }
    }
}