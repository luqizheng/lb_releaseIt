namespace ReleaseIt.Executor
{
    public interface IExecutor
    {
        void Invoke(ICommand command, ExecuteSetting executeSetting);
    }
}