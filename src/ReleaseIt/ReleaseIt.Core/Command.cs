using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ReleaseIt
{
    public abstract class Command<T> : ICommand where T : Setting
    {
        private readonly T _setting;

        private string _id;

        protected Command(T setting)
        {
            _setting = setting;
        }



        /// <summary>
        /// </summary>
        public T Setting
        {
            get { return _setting; }
        }

        public string Id
        {
            get { return _id ?? _setting.Id; }
            set { _id = value; }
        }

        /// <summary>
        /// </summary>
        /// <param name="tags"></param>
        /// <returns></returns>
        public bool HasTag(IEnumerable<string> tags)
        {
            if (tags == null || Setting.Tags == null)
                return false;
            return (from beCheckedTag in tags
                    from tag in Setting.Tags
                    where tag.ToLower() == beCheckedTag.ToLower()
                    select beCheckedTag).Any();
        }

        /// <summary>
        /// </summary>
        /// <param name="executeSetting"></param>
        /// <returns></returns>
        public ExecuteSetting Invoke(ExecuteSetting executeSetting)
        {
            if (executeSetting == null) throw new ArgumentNullException("executeSetting");
            var now = DateTime.Now;
            executeSetting.Setting.CommandLogger.Info("Start to run <" + Id + ">");
            var res = (ExecuteSetting)executeSetting.Clone();
            InvokeByNewSetting(res, Setting);
            executeSetting.Setting.CommandLogger.Info("Complete to run <" + Id + "> elapsed time:" +
                                                      (DateTime.Now - now).TotalSeconds);
            return res;
        }

        /// <summary>
        /// </summary>
        public bool SettingChanged { get; set; }

        /// <summary>
        /// </summary>
        /// <param name="txt"></param>
        /// <param name="setting"></param>
        public virtual void OnOutput(string txt, ExecuteSetting setting)
        {
            setting.Setting.ProcessLogger.Info(txt);
        }

        /// <summary>
        /// </summary>
        /// <param name="txt"></param>
        /// <param name="setting"></param>
        public virtual void OnErrorOutput(string txt, ExecuteSetting setting)
        {
            setting.Setting.ProcessLogger.Error(txt);
        }

        /// <summary>
        /// </summary>
        Setting ICommand.Setting
        {
            get { return Setting; }
        }

        /// <summary>
        /// </summary>
        /// <param name="executeSetting"></param>
        /// <param name="setting"></param>
        protected abstract void InvokeByNewSetting(ExecuteSetting executeSetting, Setting setting);


        public override string ToString()
        {
            return Id;
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }

        public abstract ICommand Clone();


    }
}