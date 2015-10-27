using System.Collections.Generic;

namespace ReleaseIt
{
    public interface ICommand
    {
        Setting Setting { get; }

        bool SettingChanged { get; set; }

        bool HasTag(IEnumerable<string> tag);

        ExecuteSetting Invoke(ExecuteSetting executeSetting);

        void OnOutput(string txt, ExecuteSetting setting);

        void OnErrorOutput(string txt, ExecuteSetting setting);
    }
}