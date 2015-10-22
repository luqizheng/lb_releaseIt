namespace ReleaseIt.SettingBuilders
{
    public class SettingBuilderBase<T, T1> where T : Setting where T1 : class
    {
        protected readonly T _setting;

        public SettingBuilderBase(T setting)
        {
            _setting = setting;
        }

        public T1 Tags(params string[] tag)
        {
            _setting.Tags = tag;
            return this as T1;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandName"></param>
        /// <returns></returns>
        public SettingBuilderBase<T, T1> Id(string commandName)
        {
            _setting.Id = commandName;
            return this;
        }
    }
}