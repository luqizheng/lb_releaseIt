namespace ReleaseIt.WindowCommand.CommandFinders
{
    public interface ICommandFinder
    {
        string Name { get; }
        string FindCmd();
    }
}