namespace ReleaseIt.SettingBuilders
{
    public class CopySettingBuilder
    {
        private readonly CopySetting _setting;

        public CopySettingBuilder(CopySetting setting)
        {
            _setting = setting;
        }

        public CopySettingBuilder Auth(string username, string password)
        {
            _setting.UserName = username;
            _setting.Password = password;
            return this;
        }

        public CopySettingBuilder Name(string commandName)
        {
            _setting.Name = commandName;
            return this;
        }
    }
}