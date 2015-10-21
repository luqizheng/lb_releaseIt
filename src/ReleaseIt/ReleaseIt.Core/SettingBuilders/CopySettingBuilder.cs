namespace ReleaseIt.SettingBuilders
{
    public class CopySettingBuilder : SettingBuilderBase<CopySetting, CopySettingBuilder>
    {
        public CopySettingBuilder(CopySetting setting)
            : base(setting)
        {
        }

        public CopySettingBuilder Auth(string username, string password)
        {
            _setting.UserName = username;
            _setting.Password = password;
            return this;
        }
    }
}