using ReleaseIt.Menus;
using ReleaseIt.Menus.SetupMenu;

namespace ReleaseIt
{
    public class Top : MenusList
    {
        

        public Top(bool isNew,string workingDir)
            : base(workingDir)
        {

            Add(new SetOrder(workingDir));
            Add(new CommandFactoryBuilder(isNew, workingDir));
            
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