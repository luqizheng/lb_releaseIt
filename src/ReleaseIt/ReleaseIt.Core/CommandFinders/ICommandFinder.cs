namespace ReleaseIt.CommandFinders
{
    public interface ICommandFinder
    {
        string Name { get; }
        string FindCmd();
    }
}