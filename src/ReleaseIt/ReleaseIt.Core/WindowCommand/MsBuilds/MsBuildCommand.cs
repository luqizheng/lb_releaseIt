using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using ReleaseIt.ParameterBuilder;
using ReleaseIt.WindowCommand.CommandFinders;

namespace ReleaseIt.WindowCommand.MsBuilds
{
    [DataContract]
    public class MsBuildCommand : ProcessCommand<BuildSetting>
    {
        private readonly Parameter _logFile = new Parameter("filelogger");


        private readonly ParameterWithValue<LogLevel> _logLevel =
            new ParameterWithValue<LogLevel>("verbosity", s => Convert.ToString(s).Substring(0, 1));


        private readonly ParameterWithValue<IList<ParameterWithValue<string>>> _properities =
            new ParameterWithValue<IList<ParameterWithValue<string>>>("p",
                d => { return string.Join(";", d.Select(a => a.Build())); });

        private readonly ParameterWithValue<List<Parameter>> _target =
            new ParameterWithValue<List<Parameter>>("t", s => string.Join(";", s.Select(a => a.Build())));

        public MsBuildCommand()
            : base(new MsBuildFinder())
        {
            _target.Value = new List<Parameter>();
            _properities.Value = new List<ParameterWithValue<string>>();
        }

        public MsBuildCommand(BuildSetting msbuild)
            : this()
        {
            Setting = msbuild;
        }


        public override string BuildArguments(ExecuteSetting executoSetting)
        {
            BuildEnviVariable(executoSetting);
            var projectFile = executoSetting.GetVaribale("%prjPath%");
            projectFile = IoExtender.WrapperPath(projectFile);

            _properities.Value.Add(CreateProperty("Configuration", Setting.BuildConfiguration ?? "Debug"));
            if (Setting.IsWeb)
            {
                BuildForWeb();
            }

            if (Setting.OutputDirectory != null)
            {
                BuildForOutDir(executoSetting);
            }

            return string.Format("{0} {1} {2} {3}",
                projectFile,
                _target.Value.Count != 0 ? _target.Build() : "",
                _properities.Build(),
                Setting.BuildLogFile ? _logFile.Build() : ""
                );
        }

        private void BuildForOutDir(ExecuteSetting executoSetting)
        {
            var outputdir = executoSetting.BuildByVariable(Setting.OutputDirectory);
            outputdir = IoExtender.GetPath(executoSetting.StartFolder, outputdir);
            executoSetting.ResultFolder = outputdir;
            executoSetting.AddVariable("%outDir%", outputdir);
            outputdir = IoExtender.WrapperPath(outputdir);
            var outputParam = CreateProperty(Setting.IsWeb ? "WebProjectOutputDir" : "outDir", outputdir);
            _properities.Value.Add(outputParam);
        }

        private void BuildForWeb()
        {
            _target.Value.Add(new Parameter("", "_CopyWebApplication"));
            _target.Value.Add(new Parameter("", "_WPPCopyWebApplication"));
            _target.Value.Add(new Parameter("", "TransformWebConfig"));
        }

        private void BuildEnviVariable(ExecuteSetting executoSetting)
        {
            executoSetting.AddVariable("%prjName%", Path.GetFileNameWithoutExtension(Setting.ProjectPath));

            var projectFile =
                executoSetting.BuildByVariable(IoExtender.GetPath(executoSetting.StartFolder, Setting.ProjectPath));
            executoSetting.AddVariable("%prjPath%", projectFile);

            executoSetting.ResultFolder = (new FileInfo(projectFile)).Directory.FullName;
            executoSetting.AddVariable("%outDir%", executoSetting.ResultFolder);
        }

        private ParameterWithValue<string> CreateProperty(string key, string value)
        {
            return new ParameterWithValue<string>(key, value)
            {
                Prefix = "",
                ValueSplitChar = "="
            };
        }
    }
}