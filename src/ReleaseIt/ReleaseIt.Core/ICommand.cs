using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReleaseIt
{
    public interface ICommand
    {
        Setting Setting { get; }

        bool SettingChanged { get; set; }

        bool IsMatch(IEnumerable<string> tag);

        ExecuteSetting Invoke(ExecuteSetting executeSetting);

        void OnOutput(string txt, ExecuteSetting setting);

        void OnErrorOutput(string txt, ExecuteSetting setting);

        
        

    }
}