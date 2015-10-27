using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using ReleaseIt.ParameterBuilder;

namespace ReleaseIt.Commands.Windows.MsBuilds
{
    [DataContract]
    public class MsBuildCommand : ProcessCommand<CompileSetting>
    {
        public MsBuildCommand(CompileSetting msbuild)
            : base(new MsBuildFinder(), msbuild)
        {
        }


        public override string BuildArguments(ExecuteSetting executoSetting)
        {
            BuildEnviVariable(executoSetting);
            var parameters = new List<ICmdParameter>
            {
                MakeProjectPath(executoSetting),
                MakeProperties(executoSetting)
            };
            MakeTargets(parameters);
            MakeLog(parameters);

            return string.Join(" ", parameters.Select(s => s.Build()));
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

        private ICmdParameter MakeProjectPath(ExecuteSetting executoSetting)
        {
            var projectFile = executoSetting.GetVaribale("%prjPath%");
            projectFile = IoExtender.WrapperPath(projectFile);


            return new Parameter("", projectFile);
        }

        private ICmdParameter MakeProperties(ExecuteSetting executoSetting)
        {
            var properities = new ParameterWithValue<IList<ParameterWithValue<string>>>("p",
                d => { return string.Join(";", d.Select(a => a.Build())); })
            {
                Value = new List<ParameterWithValue<string>>()
            };
            properities.Value.Add(CreateProperty("Configuration", Setting.BuildConfiguration ?? "Debug"));

            if (Setting.OutputDirectory != null)
            {
                var outputdir = executoSetting.BuildByVariable(Setting.OutputDirectory);
                outputdir = IoExtender.GetPath(executoSetting.StartFolder, outputdir);
                executoSetting.ResultFolder = outputdir;
                executoSetting.AddVariable("%outDir%", outputdir);
                outputdir = IoExtender.WrapperPath(outputdir);
                var outputParam = CreateProperty(Setting.IsWeb ? "WebProjectOutputDir" : "outDir", outputdir);
                properities.Value.Add(outputParam);
            }

            return properities;
        }

        private void MakeTargets(IList<ICmdParameter> parameters)
        {
            if (Setting.IsWeb)
            {
                var target = new ParameterWithValue<List<Parameter>>("t",
                    s => string.Join(";", s.Select(a => a.Build())))
                {
                    Value = new List<Parameter>()
                };
                target.Value.Add(new Parameter("", "_CopyWebApplication"));
                target.Value.Add(new Parameter("", "_WPPCopyWebApplication"));
                target.Value.Add(new Parameter("", "TransformWebConfig"));

                parameters.Add(target);
            }
        }

        private void MakeLog(IList<ICmdParameter> parameters)
        {
            var logLevel =
                 new ParameterWithValue<LogLevel>("verbosity",
                     s => Convert.ToString(s).Substring(0, 1));
            logLevel.Value = LogLevel.minimal;
            parameters.Add(logLevel);

            if (Setting.BuildLogFile)
            {
                var logFile = new Parameter("filelogger");
                parameters.Add(logFile);
            }
        }
    }
}