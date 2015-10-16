using System.ComponentModel;
using System.Runtime.Serialization;

namespace ReleaseIt
{
    [DataContract]
    [Description(@"Build Setting,it output variable 
%outDir% and %result% is compiler directory.%prjName% is project name without extension and path.%prjPath% is project full path")]
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

    public enum LogLevel
    {
        notSepcial,
        quite,
        minimal,
        normal,
        detailed,
        diagnostic
    }
}