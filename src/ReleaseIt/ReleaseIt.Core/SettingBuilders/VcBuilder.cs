using System;

namespace ReleaseIt.SettingBuilders
{
    public class VcBuilder : SettingBuilderBase<VersionControlSetting>
    {
        public VcBuilder(VersionControlSetting setting)
            : base(setting)
        {
        }

        public VcBuilder Url(string url)
        {
            if (url == null) throw new ArgumentNullException("url");
            _setting.Url = url;
            return this;
        }

        public VcBuilder Auth(string username, string password)
        {
            if (string.IsNullOrEmpty(username)) throw new ArgumentNullException("username");
            if (string.IsNullOrEmpty(password)) throw new ArgumentNullException("password");
            _setting.UserName = username;
            _setting.Password = password;
            return this;
        }

        public VcBuilder WorkingCopy(string workingCopy)
        {
            _setting.WorkingCopy = workingCopy;
            return this;
        }
    }
}