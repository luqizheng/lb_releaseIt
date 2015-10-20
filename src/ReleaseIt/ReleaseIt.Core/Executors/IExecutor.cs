namespace ReleaseIt.Executors
{
    public interface IExecutor
    {
        void Invoke<T>(ProcessCommand<T> command, ExecuteSetting setting)
            where T : Setting;
    }
}