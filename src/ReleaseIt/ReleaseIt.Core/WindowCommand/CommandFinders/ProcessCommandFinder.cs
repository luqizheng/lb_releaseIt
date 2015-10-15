using ReleaseIt.Executor;

namespace ReleaseIt.WindowCommand.CommandFinders
{
    public abstract class ProcessCommandFinder : ICommandFinder
    {
        private IExecutor _executor;
        public abstract string Name { get; }
        public abstract string FindCmd();

        public IExecutor Executor
        {
            get { return _executor ?? (_executor = new ProcessExecutor()); }
            set { _executor = value; }
        }
    }
}