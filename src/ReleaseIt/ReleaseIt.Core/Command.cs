using System;
using System.Linq;

namespace ReleaseIt
{
    public abstract class Command<T> : ICommand where T : Setting
    {

        public T Setting { get; protected set; }


        public bool IsMatch(string[] tags)
        {
            foreach (var tag in this.Setting.Tags)
            {
                if (this.Setting.Tags.Contains(tag))
                    return true;
            }
            return false;
        }

        public string From
        {
            get
            {
                if (Setting.From == null)
                {
                    return CommandSet.DefaultExecuteSetting;
                }
                return Setting.From;
            }
            set { Setting.From = value; }
        }

        public ExecuteSetting Invoke(ExecuteSetting executeSetting)
        {
            if (executeSetting == null) throw new ArgumentNullException("executeSetting");
            var res = (ExecuteSetting)executeSetting.Clone();
            InvokeByNewSetting(res);
            return res;

        }

        protected abstract void InvokeByNewSetting(ExecuteSetting executeSetting);

        public bool SettingChanged { get; set; }
        public virtual void OnOutput(string txt)
        {
            Console.WriteLine(txt);
        }

        public virtual void OnErrorOutput(string txt)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(txt);
        }


        Setting ICommand.Setting
        {
            get { return Setting; }
        }
    }
}