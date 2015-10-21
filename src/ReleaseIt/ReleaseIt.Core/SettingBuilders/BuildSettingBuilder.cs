namespace ReleaseIt.SettingBuilders
{
    public class BuildSettingBuilder : SettingBuilderBase<BuildSetting, BuildSettingBuilder>
    {
        /// <summary>
        /// </summary>
        /// <param name="setting"></param>
        public BuildSettingBuilder(BuildSetting setting)
            : base(setting)
        {
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public BuildSettingBuilder Release()
        {
            return BuildConfiguration("Release");
        }

        /// <summary>
        ///     Build Configruation for Debug
        /// </summary>
        /// <returns></returns>
        public BuildSettingBuilder Debug()
        {
            return BuildConfiguration("Debug");
        }

        /// <summary>
        /// </summary>
        /// <param name="setting"></param>
        /// <returns></returns>
        public BuildSettingBuilder BuildConfiguration(string setting)
        {
            _setting.BuildConfiguration = setting;
            return this;
        }

        /// <summary>
        /// </summary>
        /// <param name="projPath"></param>
        /// <returns></returns>
        public BuildSettingBuilder ProjectPath(string projPath)
        {
            _setting.ProjectPath = projPath;
            return this;
        }

        /// <summary>
        /// </summary>
        /// <param name="outputDir"></param>
        /// <returns></returns>
        public BuildSettingBuilder CopyTo(string outputDir)
        {
            _setting.OutputDirectory = outputDir;
            return this;
        }

     
    }
}