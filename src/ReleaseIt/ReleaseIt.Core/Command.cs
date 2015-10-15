namespace ReleaseIt
{
    public abstract class Command<T> : ICommand
    {

        public T Setting { get; protected set; }
        public abstract void Invoke(ExecuteSetting executeSetting);


        object ICommand.Setting
        {
            get { return Setting; }
        }
    }
}