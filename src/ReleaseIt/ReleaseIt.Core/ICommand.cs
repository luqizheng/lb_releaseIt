using System.Collections.Generic;

namespace ReleaseIt
{
    public interface ICommand
    {
        Setting Setting { get; }

        bool SettingChanged { get; set; }

        bool IsMatch(IEnumerable<string> tag);

        ExecuteSetting Invoke(ExecuteSetting executeSetting);

        void OnOutput(string txt);

        void OnErrorOutput(string txt);

    }
}