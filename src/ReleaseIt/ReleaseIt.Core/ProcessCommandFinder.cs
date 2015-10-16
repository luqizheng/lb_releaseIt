using ReleaseIt.WindowCommand.CommandFinders;

namespace ReleaseIt
{
    public abstract class ProcessCommandFinder : ICommandFinder
    {
        public abstract string Name { get; }
        public abstract string FindCmd();
    }
}