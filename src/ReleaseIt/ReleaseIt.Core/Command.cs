using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using ReleaseIt.WindowCommand.CommandFinders;

namespace ReleaseIt
{
    [DataContract]
    public abstract class Command<T> : ICommand
    {
        protected Command(ICommandFinder finder)
        {
            if (finder == null) throw new ArgumentNullException("finder");
            Finder = finder;
        }

        [JsonIgnore]
        public ICommandFinder Finder { get; set; }

        public abstract T Setting { get; protected set; }

        public abstract string BuildArguments(ExecuteSetting executoSetting);

        public string GetCommand(ExecuteSetting executorSetting)
        {
            return Finder.FindCmd();
        }

        object ICommand.Setting
        {
            get { return Setting; }
        }
    }
}