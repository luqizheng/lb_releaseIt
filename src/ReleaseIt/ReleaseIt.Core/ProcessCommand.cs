using System;
using Newtonsoft.Json;
using ReleaseIt.WindowCommand.CommandFinders;

namespace ReleaseIt
{
    public abstract class ProcessCommand<T> : Command<T> where T : Setting
    {
        protected ProcessCommand(ICommandFinder finder)
        {
            if (finder == null) throw new ArgumentNullException("finder");
            Finder = finder;
        }


     
        public ICommandFinder Finder { get; set; }

        protected override void InvokeByNewSetting(ExecuteSetting executeSetting)
        {
            executeSetting.Setting.Executor.Invoke(this, executeSetting);
        }


        public abstract string BuildArguments(ExecuteSetting executoSetting);

        public string GetCommand(ExecuteSetting executorSetting)
        {
            return Finder.FindCmd();
        }
    }
}