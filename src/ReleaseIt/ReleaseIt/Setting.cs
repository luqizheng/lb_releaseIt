using System.Collections.Generic;

namespace ReleaseIt
{
    public abstract class Setting
    {
        internal static Setting ExitSaveOrNot = new Exist();
        private IDictionary<string, Setting> _commands;


        public abstract string Key { get; }
        public abstract string Description { get; }

        public Setting Parent { get; private set; }

        protected IDictionary<string, Setting> Commands
        {
            get { return _commands ?? (_commands = new SortedDictionary<string, Setting>()); }
        }

        public void Add(Setting command)
        {
            command.Parent = this;
            Commands.Add(command.Key, command);
        }


        public abstract Setting Execute(CommandFactory commandFactory);
    }
}