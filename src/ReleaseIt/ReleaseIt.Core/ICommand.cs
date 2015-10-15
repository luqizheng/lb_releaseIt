using ReleaseIt.WindowCommand.CommandFinders;

namespace ReleaseIt
{
    public interface ICommand
    {
        object Setting { get; }

        void Invoke(ExecuteSetting executeSetting);
    }
}