
namespace ReleaseIt.WindowCommand.CommandFinders
{
    public abstract class ProcessCommandFinder : ICommandFinder
    {
       
        public abstract string Name { get; }
        public abstract string FindCmd();

    }
}