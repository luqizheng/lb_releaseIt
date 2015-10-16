using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReleaseIt.SettingBuilders
{
    public class VCBuilder
    {
        private readonly VersionControlSetting _setting;

        public VCBuilder(VersionControlSetting setting)
        {
            _setting = setting;
        }

        public VCBuilder Url(string url)
        {
            _setting.Url = url;
            ;
            return this;
        }

        public VCBuilder Auth(string username, string password)
        {
            _setting.UserName = username;
            _setting.Password = password;
            return this;
        }

        public VCBuilder WorkingCopy(string workingCopy)
        {
            _setting.WorkingCopy = workingCopy;
            return this;
        }

        public VCBuilder Name(string commandName)
        {
            _setting.Name = commandName;
            return this;
        }
    }
}
