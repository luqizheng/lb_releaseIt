using ReleaseIt.WindowCommand.CommandFinders;

namespace ReleaseIt
{
    public interface ICommand
    {
        object Setting { get; }

        ICommandFinder Finder { get; }
        string BuildArguments(ExecuteSetting executoSetting);
        string GetCommand(ExecuteSetting executorSetting);
    }
}