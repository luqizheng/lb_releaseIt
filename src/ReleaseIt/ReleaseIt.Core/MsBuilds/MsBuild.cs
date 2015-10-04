using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ReleaseIt.CommandFinders;
using ReleaseIt.ParameterBuilder;

namespace ReleaseIt.MsBuilds
{
    public class MsBuild : Command
    {
        private readonly ParameterWithValue<string> _configuration =
            new ParameterWithValue<string>("/p");

        private readonly Parameter _logFile = new Parameter("filelogger");


        private readonly ParameterWithValue<LogLevel> _logLevel =
            new ParameterWithValue<LogLevel>("/verbosity", s => Convert.ToString(s).Substring(0, 1));

        private readonly ParameterWithValue<IList<ParameterWithValue<string>>> _properities =
            new ParameterWithValue<IList<ParameterWithValue<string>>>("/p",
                d => { return string.Join(";", d.Select(a => a.Build())); });

        private readonly ParameterWithValue<string> _traget =
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
            get { return _traget.Value.Split(';'); }
            set { _traget.Value = string.Join(";", value); }
        }

        /// <summary>
        ///     /p:
        ///     Release/Debug or soemthing you defined
        /// </summary>
        public string Configuration
        {
            get { return _configuration.Value; }
            set { _configuration.Value = value; }
        }

        /// <summary>
        ///     Project Value soulation
        /// </summary>
        public string ProjectPath { get; set; }

        /// <summary>
        ///     /p 参数
        /// </summary>
        public void AddProperty(string name, string vlaue)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (vlaue == null) throw new ArgumentNullException("vlaue");
            var cmd = new ParameterWithValue<string>(name)
            {
                ValueSplitChar = "=",
                Value = vlaue
            };
            _properities.Value.Add(cmd);
        }

        protected override ICmdParameter[] BuildParameters(string executeFolderName)
        {
            var projectPath = Path.Combine(executeFolderName, ProjectPath);
            if (!File.Exists(projectPath))
            {
                throw new FileNotFoundException("Can't find the project file.", projectPath);
            }
            var outDir = (from prop in _properities.Value where prop.Name
                              == "WebProjectOutputDir"  select prop).FirstOrDefault();
            if (outDir != null)
            {
                outDir.Value = Path.Combine(executeFolderName, outDir.Value);
            }
            var result = new ICmdParameter[]
            {
                new Parameter("", projectPath),
                _properities,
                _traget,
                _configuration,
                _logFile,
                _logLevel
            };
            return result;
        }

        protected override ExceuteResult CreateResult(string executeFolder)
        {
            return new ExceuteResult
            {
                ResultPath = executeFolder
            };
        }
    }
}