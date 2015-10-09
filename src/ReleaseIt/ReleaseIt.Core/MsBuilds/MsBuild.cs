using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using ReleaseIt.CommandFinders;
using ReleaseIt.ParameterBuilder;

namespace ReleaseIt.MsBuilds
{
    public class MsBuild : Command
    {
        private readonly Parameter _logFile = new Parameter("filelogger");


        private readonly ParameterWithValue<LogLevel> _logLevel =
            new ParameterWithValue<LogLevel>("/verbosity", s => Convert.ToString(s).Substring(0, 1));

        private readonly ParameterWithValue<IList<ParameterWithValue<string>>> _properities =
            new ParameterWithValue<IList<ParameterWithValue<string>>>("/p",
                d => { return string.Join(";", d.Select(a => a.Build())); });

        private readonly ParameterWithValue<string> _target =
            new ParameterWithValue<string>("/t");


        public MsBuild()
            : base(new MsBuildFinder())
        {
            _properities.Value = new List<ParameterWithValue<string>>();
        }


        public bool BuildLogFile
        {
            get { return _logFile.HasSet; }
            set { _logFile.HasSet = value; }
        }

        public LogLevel LogLevel
        {
            get { return _logLevel.Value; }
            set { _logLevel.Value = value; }
        }

        /// <summary>
        ///     msbuild target /t:
        /// </summary>
        public string[] Target
        {
            get
            {
                if (_target.Value != null)
                    return _target.Value.Split(';');
                return new string[0];
            }
            set { _target.Value = string.Join(";", value); }
        }


        public string[] Properties
        {
            get
            {
                var str = new List<string>(_properities.Value.Count);
                str.AddRange(_properities.Value.Select(item => item.ToString()));
                return str.ToArray();
            }
            set
            {
                _properities.Value.Clear();
                foreach (var str in value)
                {
                    var ary = str.Split('=');
                    if (ary.Length != 2)
                    {
                        throw new ArgumentOutOfRangeException("value", "It should be Key=Value");
                    }
                    var name = ary[0];
                    var val = ary[1];
                    var cmd = new ParameterWithValue<string>(name)
                    {
                        ValueSplitChar = "=",
                        Value = val
                    };
                    _properities.Value.Add(cmd);
                }
            }
        }

        /// <summary>
        ///     Project Value soulation
        /// </summary>
        public string ProjectPath { get; set; }

        /// <summary>
        ///     /p 参数
        /// </summary>
        public void AddProperty(string name, string val)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (val == null) throw new ArgumentNullException("val");
            var cmd = new ParameterWithValue<string>(name)
            {
                ValueSplitChar = "=",
                Value = val
            };
            _properities.Value.Add(cmd);
        }

        protected override ICmdParameter[] BuildParameters(ExceuteResult executeResult)
        {
            var projectPath = IoExtender.GetPath(executeResult.StartFolder, ProjectPath);
            if (!File.Exists(projectPath))
            {
                throw new FileNotFoundException("Can't find the project file.", projectPath);
            }
            var outDir = FindOutputDir();
            outDir.Value = IoExtender.GetPath(executeResult.StartFolder, outDir.Value);

            if (projectPath.Contains(" "))
            {
                projectPath = "\"" + projectPath + "\"";
            }
            var result = new ICmdParameter[]
            {
                new Parameter("", projectPath),
                _properities,
                _target,
                _logFile,
                _logLevel
            };
            return result;
        }


        protected override void BeforeInvoke(ProcessStartInfo startInfo)
        {
            var path = Path.GetFileNameWithoutExtension(ProjectPath);
            startInfo.EnvironmentVariables["prjName"] = path;
            base.BeforeInvoke(startInfo);
        }

        private ParameterWithValue<string> FindOutputDir()
        {
            return (from prop in _properities.Value
                where prop.Name
                      == "WebProjectOutputDir"
                select prop).FirstOrDefault();
        }

        protected override void
            UpdateExecuteResultInfo(ExceuteResult result)
        {
            result.ExecuteFile = IoExtender.GetPath(result.StartFolder, ProjectPath);

            var outDir = FindOutputDir();
            if (outDir != null)
                result.ResultFolder = outDir.Value;
        }
    }
}