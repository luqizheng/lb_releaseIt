using System.Collections.Generic;
using System.IO;

namespace ReleaseIt
{
    public class CommandSet
    {
        private readonly IList<ICommand> _commands;
        private readonly string _workDirectory;

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
                cmd.Invoke(executeResult);
            }
        }

        public void Add(object setting)
        {
            var command = Setting.Create(setting);
            Commands.Add(command);
        }
    }
}