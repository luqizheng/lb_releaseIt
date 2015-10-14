using System.Collections.Generic;
using System.IO;
using ReleaseIt.Executor;
using ReleaseIt.WindowCommand;

namespace ReleaseIt
{
    public interface ICommand
    {
        string BuildArguments(ExecuteSetting executoSetting);
        string GetCommand(ExecuteSetting executorSetting);
    }

    public class CommandSet
    {
        private readonly IList<ICommand> _commands;
        private readonly string _workDirectory;
        private IExecutor _executor;

        public CommandSet(string dirDirecotry)
            : this(dirDirecotry, new List<ICommand>())
        {
        }

        public CommandSet(string workDirectory, IList<ICommand> commands)
        {
            _workDirectory = workDirectory;
            _commands = commands;
        }

        public ConfigurationSetting Setting { get; set; }

        internal IExecutor Executor
        {
            get { return _executor ?? (_executor = new ProcessExecutor()); }
            set { _executor = value; }
        }

        public IList<ICommand> Commands
        {
            get { return _commands; }
        }

        public void Invoke()
        {
            if (!Directory.Exists(_workDirectory))
            {
                (new DirectoryInfo(_workDirectory)).CreateEx();
            }

            var executeResult = new ExecuteSetting(_workDirectory);
            foreach (var cmd in _commands)
            {
                Executor.Invoke(cmd, executeResult);
            }
        }

        public void Add(object setting)
        {
            var command = Setting.Create(setting);
            Commands.Add(command);
        }

     
    }
}