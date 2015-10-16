namespace ReleaseIt.Arguments
{
    public abstract class ArgumentHandler
    {
        public abstract string Key { get; }

        public abstract bool Handle(CommandSet set, string fileName, string argument);
    }
}