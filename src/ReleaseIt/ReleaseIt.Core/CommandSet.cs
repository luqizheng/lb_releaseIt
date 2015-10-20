using System;
using System.Collections.Generic;
using System.IO;
using ReleaseIt.IniStore;

namespace ReleaseIt
{
    public class CommandSet
    {
        private readonly IList<ICommand> _commands;

        private List<string> _run;

        internal const string DefaultExecuteSetting = "default";
        private List<string> _skip;
        private Dictionary<string, ExecuteSetting> _settings = new Dictionary<string, ExecuteSetting>();
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
            _settings.Add(DefaultExecuteSetting, setting);
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
            var setting = _settings[DefaultExecuteSetting];
            if (!Directory.Exists(setting.WorkDirectory))
            {
                (new DirectoryInfo(setting.WorkDirectory)).CreateEx();
            }

            bool settingChanged = false;
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

                cmd.Invoke(_settings[cmd.From]);

                settingChanged = cmd.SettingChanged || settingChanged;
            }

            if (settingChanged)
            {
                if (OnCommandSettingChanged != null)
                {
                    OnCommandSettingChanged(this, EventArgs.Empty);
                }
            }
        }

        public event EventHandler OnCommandSettingChanged;

        public ICommand Add(Setting setting)
        {
            var command = _settings[DefaultExecuteSetting].Setting.Create(setting);
            Commands.Add(command);
            return command;
        }
    }
}