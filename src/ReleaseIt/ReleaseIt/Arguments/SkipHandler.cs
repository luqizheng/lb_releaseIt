namespace ReleaseIt.Arguments
{
    public class SkipHandler : ArgumentHandler
    {
        public override string Key
        {
            get { return "skip"; }
        }

        public override bool Handle(CommandSet set, string fileName, string argument)
        {
            var f = argument.Split(':');
            if (f.Length == 1)
                return true;

            var commandNames = f[1].Split(';');
            set.Skip.AddRange(commandNames);
            return true;
        }


    }
}