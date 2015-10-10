namespace ReleaseIt
{
    public class BuildSetting
    {
        public BuildSetting()
        {
            //_properities.Value = new List<ParameterWithValue<string>>();
            BuildConfiguration = "Debug";
        }

        public bool IsWeb { get; set; }
        public bool BuildLogFile { get; set; }

        public LogLevel LogLevel { get; set; }

        public string OutputDirectory { get; set; }

        public string ProjectPath { get; set; }

        public string BuildConfiguration { get; set; }
    }

    public static class BuildSettingExtend
    {
        public static BuildSettingBuilder Build(this CommandSet set, bool isWeb)
        {
            var buildSetting = new BuildSetting()
            {
                IsWeb = isWeb
            };
            set.Add(buildSetting);
            return new BuildSettingBuilder(buildSetting);
        }
    }

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
            this._setting.OutputDirectory = outputDir;
            return this;
        }
    }
}