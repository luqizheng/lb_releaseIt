using System;
using System.Collections.Generic;

namespace ReleaseIt.Menus
{
    public abstract class Menu
    {
        internal static readonly Menu ExitSaveOrNot = new Exist(Environment.CurrentDirectory);
        private IDictionary<string, Menu> _settings;

        protected Menu(string workingFolder)
        {
            WorkingFolder = workingFolder;
        }

        protected string WorkingFolder { get; private set; }

        public abstract string Key { get; }
        public abstract string Description { get; }

        protected Menu Parent { get; private set; }

        protected IDictionary<string, Menu> Settings
        {
            get { return _settings ?? (_settings = new SortedDictionary<string, Menu>()); }
        }

        protected void Add(Menu command)
        {
            command.Parent = this;
            Settings.Add(command.Key, command);
        }


        public abstract Menu Do(CommandSet commandSet, string resultPath);


        public virtual string GetResultPath(Command command)
        {
            return WorkingFolder;
        }
    }
}