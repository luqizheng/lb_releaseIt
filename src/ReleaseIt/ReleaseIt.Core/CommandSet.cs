using System.Collections.Generic;
using System.IO;

namespace ReleaseIt
{
    public class CommandSet
    {
        private readonly IList<ICommand> _commands;
        private readonly string _workDirectory;

        private List<string> _run;

        private List<string> _skip;

        public CommandSet(string dirDirecotry)
            : this(dirDirecotry, new List<ICommand>())
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="workDirectory"></param>
        /// <param name="commands"></param>
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

        public List<string> Skip
        {
            get { return _skip ?? (_skip = new List<string>()); }
        }

        public List<string> Run
        {
            get { return _run ?? (_run = new List<string>()); }
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
                if (Skip.Count != 0 && Skip.Contains(cmd.Setting.Name))
                {
                    continue;
                }
                if (Run.Count != 0 && !Run.Contains(cmd.Setting.Name))
                {
                    continue;
                }
                cmd.Invoke(executeResult);
            }
        }

        public ICommand Add(Setting setting)
        {
            var command = Setting.Create(setting);
            Commands.Add(command);
            return command;
        }
    }
}