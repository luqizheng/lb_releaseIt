namespace ReleaseIt
{
    public interface ICommand
    {
        Setting Setting { get; }

        bool SettingChanged { get; set; }

        void Invoke(ExecuteSetting executeSetting);

        void OnOutput(string txt);

        void OnErrorOutput(string txt);
    }
}