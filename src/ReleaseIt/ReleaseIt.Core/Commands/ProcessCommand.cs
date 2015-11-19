using System;
using ReleaseIt.WindowCommand.CommandFinders;

namespace ReleaseIt.Commands
{
    public abstract class ProcessCommand<T> : Command<T> where T : Setting
    {
        protected ProcessCommand(ICommandFinder finder, T setting)
            : base(setting)
        {
            if (finder == null) throw new ArgumentNullException("finder");
            Finder = finder;
        }


        public ICommandFinder Finder { get; set; }

        protected override void InvokeByNewSetting(ExecuteSetting executeSetting, Setting setting)
        {
            var cmd = this as ICommand;
            executeSetting.Setting.Executor.Invoke<T>(cmd, executeSetting);
        }


        public abstract string BuildArguments(ExecuteSetting executoSetting);

        public string GetCommand(ExecuteSetting executorSetting)
        {
            return Finder.FindCmd();
        }
    }
}