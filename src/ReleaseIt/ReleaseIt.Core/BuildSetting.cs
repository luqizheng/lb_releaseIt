using System.Runtime.Serialization;

namespace ReleaseIt
{
      [DataContract]
    public class BuildSetting : Setting
    {
        public BuildSetting()
        {
            //_properities.Value = new List<ParameterWithValue<string>>();
            BuildConfiguration = "Debug";
        }
          [DataMember]
        public bool IsWeb { get; set; }
          [DataMember]
        public bool BuildLogFile { get; set; }
          [DataMember]
        public LogLevel LogLevel { get; set; }
          [DataMember]
        public string OutputDirectory { get; set; }
          [DataMember]
        public string ProjectPath { get; set; }
          [DataMember]
        public string BuildConfiguration { get; set; }
    }

    public static class BuildSettingExtend
    {
        public static BuildSettingBuilder Build(this CommandSet set, bool isWeb)
        {
            var buildSetting = new BuildSetting
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
    }
}