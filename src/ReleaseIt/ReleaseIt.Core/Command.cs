using System;
using System.Collections.Generic;
using System.Linq;

namespace ReleaseIt
{
    public abstract class Command<T> : ICommand where T : Setting
    {

        public T Setting { get; protected set; }


        public bool IsMatch(IEnumerable<string> tags)
        {
            if (tags == null || this.Setting.Tags == null)
                return false;
            return (from beCheckedTag in tags
                    from tag in this.Setting.Tags
                    where tag.ToLower() == beCheckedTag.ToLower()
                    select beCheckedTag).Any();
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