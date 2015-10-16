using System;
using Newtonsoft.Json;
using ReleaseIt.WindowCommand;
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


        [JsonIgnore]
        public ICommandFinder Finder { get; set; }

        public override void Invoke(ExecuteSetting executeSetting)
        {

            executeSetting.Setting.IExecutor.Invoke(this, executeSetting);
        }


        public abstract string BuildArguments(ExecuteSetting executoSetting);

        public string GetCommand(ExecuteSetting executorSetting)
        {
            return Finder.FindCmd();
        }
    }
}