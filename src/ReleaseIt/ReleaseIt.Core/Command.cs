namespace ReleaseIt
{
    public abstract class Command<T> : ICommand where T : Setting
    {
        public T Setting { get; protected set; }
        public abstract void Invoke(ExecuteSetting executeSetting);
        public bool SettingChanged { get; set; }


        Setting ICommand.Setting
        {
            get { return Setting; }
        }
    }

}