using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace ReleaseIt
{
    public abstract class Command<T> : ICommand where T : Setting
    {
        public T Setting { get; protected set; }
        [Description("Dependcies.pre-condition of this step")]
        public string Dependcies
        {
            get
            {
                if (Setting.Dependcies == null)
                {
                    return CommandSet.DefaultExecuteSetting;
                }
                return Setting.Dependcies;
            }
            set { Setting.Dependcies = value; }
        }


        public bool IsMatch(IEnumerable<string> tags)
        {
            if (tags == null || Setting.Tags == null)
                return false;
            return (from beCheckedTag in tags
                from tag in Setting.Tags
                where tag.ToLower() == beCheckedTag.ToLower()
                select beCheckedTag).Any();
        }


        public ExecuteSetting Invoke(ExecuteSetting executeSetting)
        {
            if (executeSetting == null) throw new ArgumentNullException("executeSetting");
            var res = (ExecuteSetting) executeSetting.Clone();
            InvokeByNewSetting(res, this.Setting);
            return res;
        }

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

        protected abstract void InvokeByNewSetting(ExecuteSetting executeSetting, Setting setting);
    }
}