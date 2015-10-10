using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ReleaseIt.ParameterBuilder;
using ReleaseIt.WindowCommand.CommandFinders;

namespace ReleaseIt.WindowCommand.MsBuilds
{
    public class MsBuildCommand : Command
    {
        private readonly Parameter _logFile = new Parameter("filelogger");


        private readonly ParameterWithValue<LogLevel> _logLevel =
            new ParameterWithValue<LogLevel>("/verbosity", s => Convert.ToString(s).Substring(0, 1));


        private readonly ParameterWithValue<IList<ParameterWithValue<string>>> _properities =
            new ParameterWithValue<IList<ParameterWithValue<string>>>("/p",
                d => { return string.Join(";", d.Select(a => a.Build())); });

        private readonly ParameterWithValue<List<Parameter>> _target =
            new ParameterWithValue<List<Parameter>>("/t", s => string.Join(";", s.Select(a => a.Build())));

        public MsBuildCommand()
            : base(new MsBuildFinder())
        {
            
        }
        public MsBuildCommand(BuildSetting msbuild)
            : base(new MsBuildFinder())
        {
            Setting = msbuild;
        }

        public BuildSetting Setting { get; set; }

        public override string BuildArguments(ExecuteSetting executoSetting)
        {
            string outputdir = null;
            if (Setting.OutputDirectory != null)
            {
                outputdir = Setting.OutputDirectory.Replace("%prjName%",
                    Path.GetFileNameWithoutExtension(Setting.ProjectPath));
            }
            _properities.Value.Add(new ParameterWithValue<string>("Configuration",
                Setting.BuildConfiguration ?? "Debug"));
            if (Setting.IsWeb)
            {
                _target.Value.Add(new Parameter("", "_CopyWebApplication"));
                _target.Value.Add(new Parameter("", "_WPPCopyWebApplication"));
                _target.Value.Add(new Parameter("", "TransformWebConfig"));
                if (outputdir != null)
                {
                    _properities.Value.Add(new ParameterWithValue<string>("WebProjectOutputDir", outputdir));
                }
            }
            else
            {
                if (outputdir != null)
                {
                    _properities.Value.Add(new ParameterWithValue<string>("outDir", outputdir));
                }
            }

            var projectFile = IoExtender.GetPath(executoSetting.StartFolder, Setting.ProjectPath);
            return string.Format("{0} {1} {2}", projectFile, _target.Build(), _properities.Build());
        }
    }
}