namespace ReleaseIt.SettingBuilders
{
    public class BuildSettingBuilder
    {
        private readonly BuildSetting _setting;

        public BuildSettingBuilder(BuildSetting setting)
        {
            _setting = setting;
        }

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

        public BuildSettingBuilder BuildConfiguration(string setting)
        {
            _setting.BuildConfiguration = setting;
            return this;
        }

        public BuildSettingBuilder ProjectPath(string projPath)
        {
            _setting.ProjectPath = projPath;
            return this;
        }


        public BuildSettingBuilder CopyTo(string outputDir)
        {
            _setting.OutputDirectory = outputDir;
            return this;
        }

        public BuildSettingBuilder Name(string commandName)
        {
            _setting.Name = commandName;
            return this;

        }
    }
}