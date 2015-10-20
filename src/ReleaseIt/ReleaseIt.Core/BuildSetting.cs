using System.ComponentModel;

namespace ReleaseIt
{
    [Description(@"Build Setting,it output variable 
%outDir% and %result% is compiler directory.%prjName% is project name without extension and path.%prjPath% is project full path"
        )]
    public class BuildSetting : Setting
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