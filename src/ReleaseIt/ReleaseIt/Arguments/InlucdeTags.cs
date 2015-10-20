namespace ReleaseIt.Arguments
{
    public class InlucdeTags : ArgumentHandler
    {
        public override string Key
        {
            get { return "t"; }
        }

        public override bool Handle(CommandSet set, string fileName, string argument)
        {
            var f = argument.Split(':');
            if (f.Length == 1)
                return true;

            var commandNames = f[1].Split(';');
            set.IncludeTags.AddRange(commandNames);
            return true;
        }
    }
}