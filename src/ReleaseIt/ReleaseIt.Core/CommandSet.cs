using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ReleaseIt
{
    public class CommandSet
    {
        internal const string DefaultExecuteSetting = "default";
        private readonly IList<ICommand> _commands;

        private readonly IList<string> _commnadIds;

        private readonly Dictionary<string, ExecuteSetting> _executeSettings
            = new Dictionary<string, ExecuteSetting>();

        private List<string> _run;
        private List<string> _skip;

        private List<string> _tags;

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
            _executeSettings.Add(DefaultExecuteSetting, setting);
            _commands = commands;
            _commnadIds = new List<string>();
        }


        private IList<ICommand> Commands
        {
            get { return _commands; }
        }

        public IEnumerable<Setting> Settings
        {
            get { return _commands.Select(s => s.Setting); }
        }

        /// <summary>
        ///     Skip command name
        /// </summary>
        public List<string> Skip
        {
            get { return _skip ?? (_skip = new List<string>()); }
        }

        /// <summary>
        ///     Include command name
        /// </summary>
        public List<string> Include
        {
            get { return _run ?? (_run = new List<string>()); }
        }

        public List<string> IncludeTags
        {
            get { return _tags ?? (_tags = new List<string>()); }
        }

        public void Invoke()
        {
            var setting = _executeSettings[DefaultExecuteSetting];
            if (!Directory.Exists(setting.WorkDirectory))
            {
                (new DirectoryInfo(setting.WorkDirectory)).CreateEx();
            }

            var settingChanged = false;
            var exeCmod = BuildExecuteQueue();

            while (exeCmod.Count != 0)
            {
                var cmd = exeCmod.Dequeue();
                var resultSetting = cmd.Invoke(_executeSettings[cmd.Setting.Dependcies ?? DefaultExecuteSetting]);
                _executeSettings.Add(cmd.Setting.Id, resultSetting);
                settingChanged = cmd.SettingChanged || settingChanged;
            }
            if (settingChanged)
            {
                OnSettingChanged();
            }
        }

        private void OnSettingChanged()
        {
            if (OnCommandSettingChanged != null)
            {
                OnCommandSettingChanged(this, EventArgs.Empty);
            }
        }

        private Queue<ICommand> BuildExecuteQueue()
        {
            var result = new List<ICommand>();
            foreach (var cmd in _commands)
            {
                if (Skip.Count != 0 && Skip.Contains(cmd.Setting.Id))
                {
                    continue;
                }
                if ((Include.Count == 0 && IncludeTags.Count == 0)
                    || Include.Contains(cmd.Setting.Id)
                    || cmd.IsMatch(IncludeTags))
                {
                    result.Add(cmd);
                    LoopPrepend(cmd, result);
                }
            }
            return new Queue<ICommand>(result);
        }

        private void LoopPrepend(ICommand cmd, List<ICommand> result)
        {
            var backCount = 1;
            while (cmd != null && cmd.Setting.Dependcies != null &&
                   cmd.Setting.Dependcies != DefaultExecuteSetting)
            {
                if (result.All(s => s.Setting.Id != cmd.Setting.Dependcies))
                {
                    var preCmd = _commands.FirstOrDefault(s => s.Setting.Id == cmd.Setting.Dependcies);
                    if (preCmd != null)
                    {
                        result.Insert(result.Count - backCount, preCmd);
                        backCount++;
                        cmd = preCmd;
                        continue;
                    }
                }
                break;
            }
        }

        public event EventHandler OnCommandSettingChanged;

        public ICommand Add(Setting setting)
        {
            if (_commnadIds.Contains(setting.Id))
            {
                throw new DuplicateIdException(setting.Id);
            }
            var command = _executeSettings[DefaultExecuteSetting].Setting.Create(setting);
            Commands.Add(command);
            _commnadIds.Add(setting.Id);
            return command;
        }

        public ICommand Insert(int index, ICommand command)
        {
            if (_commnadIds.Contains(command.Setting.Id))
            {
                throw new DuplicateIdException(command.Setting.Id);
            }
            Commands.Insert(index, command);
            _commnadIds.Add(command.Setting.Id);
            return command;
        }

        public ICommand Add(ICommand command)
        {
            if (_commnadIds.Contains(command.Setting.Id))
            {
                throw new DuplicateIdException(command.Setting.Id);
            }
            Commands.Add(command);
            _commnadIds.Add(command.Setting.Id);
            return command;
        }
    }

    public class DuplicateIdException :
        Exception
    {
        public DuplicateIdException(string id)
            : base(string.Format("id={0} is exist in command set.", id))
        {
        }
    }
}