using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace ReleaseIt
{
    public interface ICommand : ICloneable
    {
        string Id { get; set; }
        Setting Setting { get; }

        bool SettingChanged { get; set; }

        bool HasTag(IEnumerable<string> tag);

        ExecuteSetting Invoke(ExecuteSetting executeSetting);

        void OnOutput(string txt, ExecuteSetting setting);

        void OnErrorOutput(string txt, ExecuteSetting setting);
   
    }

}