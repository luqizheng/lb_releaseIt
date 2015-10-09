namespace ReleaseIt
{
    public interface ICommand
    {
        ExceuteResult Invoke(ExceuteResult executeResult, CommandSet commandSet);
    }
}