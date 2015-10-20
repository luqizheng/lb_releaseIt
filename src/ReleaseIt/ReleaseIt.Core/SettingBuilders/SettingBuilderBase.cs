namespace ReleaseIt.SettingBuilders
{
    public class SettingBuilderBase<T> where T : Setting
    {
        protected readonly T _setting;

        public SettingBuilderBase(T setting)
        {
            _setting = setting;
        }

        public SettingBuilderBase<T> Name(string commandName)
        {
            _setting.Name = commandName;
            return this;
        }

        public SettingBuilderBase<T> Tags(params string[] tag)
        {
            _setting.Tags = tag;
            return this;
        }
    }
}