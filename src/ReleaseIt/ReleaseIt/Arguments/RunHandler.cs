namespace ReleaseIt.Arguments
{
    public class RunHandler : ArgumentHandler
    {
        public override string Key
        {
            get { return "run"; }
        }

        public override bool Handle(CommandSet set, string fileName, string argument)
        {
            var f = argument.Split(':');
            if (f.Length == 1)
                return true;

            var commandNames = f[1].Split(new[] { ';', ',' });
            set.Include.AddRange(commandNames);
            return true;
        }
    }
}