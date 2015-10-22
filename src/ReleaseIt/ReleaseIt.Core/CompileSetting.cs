using System.ComponentModel;

namespace ReleaseIt
{
    [Description(@"Build Setting,it output variable 
%outDir% and %result% is compiler directory.%prjName% is project name without extension and path.%prjPath% is project full path"
        )]
    public class CompileSetting : Setting
    {
        public CompileSetting()
        {
            //_properities.Value = new List<ParameterWithValue<string>>();
            BuildConfiguration = "Debug";
        }

        [Description(@"Is Web Project. True/False")]
        public bool IsWeb { get; set; }

        [Description(@"Build File or Not  True/False")]
        public bool BuildLogFile { get; set; }

        [Description(@"Log level.  notSepcial,quite,minimal,normal,detailed,diagnostic")]
        public LogLevel LogLevel { get; set; }

        [Description(@"out put path. Command copy the file to target path.")]
        public string OutputDirectory { get; set; }

        [Description(@"Project file")]
        public string ProjectPath { get; set; }

        [Description(@"something like Release Debug.")]
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