namespace ReleaseIt.SettingBuilders
{
    public class CompileSettingBuilder : SettingBuilderBase<CompileSetting, CompileSettingBuilder>
    {
        /// <summary>
        /// </summary>
        /// <param name="setting"></param>
        public CompileSettingBuilder(CompileSetting setting)
            : base(setting)
        {
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public CompileSettingBuilder Release()
        {
            return BuildConfiguration("Release");
        }

        /// <summary>
        ///     Build Configruation for Debug
        /// </summary>
        /// <returns></returns>
        public CompileSettingBuilder Debug()
        {
            return BuildConfiguration("Debug");
        }

        /// <summary>
        /// </summary>
        /// <param name="setting"></param>
        /// <returns></returns>
        public CompileSettingBuilder BuildConfiguration(string setting)
        {
            _setting.BuildConfiguration = setting;
            return this;
        }

        /// <summary>
        /// </summary>
        /// <param name="projPath"></param>
        /// <returns></returns>
        public CompileSettingBuilder ProjectPath(string projPath)
        {
            _setting.ProjectPath = projPath;
            return this;
        }

        /// <summary>
        /// </summary>
        /// <param name="outputDir"></param>
        /// <returns></returns>
        public CompileSettingBuilder CopyTo(string outputDir)
        {
            _setting.OutputDirectory = outputDir;
            return this;
        }

     
    }
}