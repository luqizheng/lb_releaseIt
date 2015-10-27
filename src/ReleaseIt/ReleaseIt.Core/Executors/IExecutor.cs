using ReleaseIt.Commands;

namespace ReleaseIt.Executors
{
    public interface IExecutor
    {
        void Invoke<T>(ICommand command, ExecuteSetting setting)
            where T : Setting;
    }
}