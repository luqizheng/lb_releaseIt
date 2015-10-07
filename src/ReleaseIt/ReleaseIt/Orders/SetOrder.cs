using System;

namespace ReleaseIt
{
    public class SetOrder : Setting
    {

        public SetOrder()
        {

        }

        public override string Key
        {
            get { return "o"; }
        }

        public override string Description
        {
            get { return "Set CofnigFile execute order."; }
        }

        public override Setting Execute(CommandFactory commandFactory)
        {
            for (var i = 0; i < commandFactory.Commands.Count; i++)
            {
                Console.WriteLine("[{0}] {1}", i, commandFactory.Commands[i]);
            }
            return this.Parent;
        }
    }
}