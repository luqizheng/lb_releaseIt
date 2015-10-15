using ReleaseIt.WindowCommand.CommandFinders;

namespace ReleaseIt
{

    public interface ICommand
    {
        Setting Setting { get; }

        void Invoke(ExecuteSetting executeSetting);
    }
}