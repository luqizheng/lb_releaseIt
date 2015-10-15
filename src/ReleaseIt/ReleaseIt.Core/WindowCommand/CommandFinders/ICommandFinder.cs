using ReleaseIt.Executor;

namespace ReleaseIt.WindowCommand.CommandFinders
{
    public interface ICommandFinder
    {
        string Name { get; }
        string FindCmd();

        IExecutor Executor { get; set; }
    }
}