using System;

namespace ReleaseIt
{
    public abstract class Command<T> : ICommand where T : Setting
    {
        public T Setting { get; protected set; }
        public abstract void Invoke(ExecuteSetting executeSetting);
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