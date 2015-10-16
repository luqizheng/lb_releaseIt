using System.Collections.Generic;
using System.IO;

namespace ReleaseIt
{
    public class CommandSet
    {
        private readonly IList<ICommand> _commands;

        private List<string> _run;
        private readonly ExecuteSetting _setting;

        private List<string> _skip;

        public CommandSet(ExecuteSetting setting)
            : this(setting, new List<ICommand>())
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="setting"></param>
        /// <param name="commands"></param>
        public CommandSet(ExecuteSetting setting, IList<ICommand> commands)
        {
            _setting = setting;
            _commands = commands;
        }


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
            if (!Directory.Exists(_setting.WorkDirectory))
            {
                (new DirectoryInfo(_setting.WorkDirectory)).CreateEx();
            }


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
                cmd.Invoke(_setting);
            }
        }

        public ICommand Add(Setting setting)
        {
            var command = _setting.Setting.Create(setting);
            Commands.Add(command);
            return command;
        }
    }
}