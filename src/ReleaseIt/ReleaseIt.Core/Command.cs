namespace ReleaseIt
{
    public abstract class Command<T> : ICommand where T : Setting
    {
        public T Setting { get; protected set; }
        public abstract void Invoke(ExecuteSetting executeSetting);


        Setting ICommand.Setting
        {
            get { return Setting; }
        }
    }
}