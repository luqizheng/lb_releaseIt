using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ReleaseIt
{
    public class ExecuteSetting : ICloneable
    {
        private readonly Dictionary<string, string> _variable = new Dictionary<string, string>();
        private string _resultFolder;
        private string _workDirectory;

        public ExecuteSetting(string startFolder)
        {
            StartFolder = startFolder;
            AddVariable("%start%", startFolder);
        }

        /// <summary>
        ///     执行完毕后的位置
        /// </summary>
        public string ResultFolder
        {
            get
            {
                return _resultFolder ?? StartFolder;
            }
            set
            {
                _resultFolder = value;
                AddVariable("%result%", _resultFolder);
            }
        }

        /// <summary>
        ///     最开始执行的位置.
        /// </summary>
        public string StartFolder { get; private set; }

        /// <summary>
        ///     执行的是文件,那么这个就有值. 如Msbuild
        /// </summary>
        public string ExecuteFile { get; set; }

        /// <summary>
        ///     工作目录
        /// </summary>
        public string WorkDirectory
        {
            get { return _workDirectory ?? StartFolder; }
            set { _workDirectory = value; }
        }


        public object Clone()
        {
            var result = new ExecuteSetting(StartFolder)
            {
                ExecuteFile = ExecuteFile,
                WorkDirectory = WorkDirectory,
                ResultFolder = ResultFolder
            };
            foreach (var item in _variable.Keys)
            {
                result.AddVariable(item, _variable[item]);
            }
            return result;
        }

        public void AddVariable(string name, string value)
        {
            name = name.ToLower();
            if (_variable.ContainsKey(name))
            {
                _variable[name] = value;
            }
            else
            {
                _variable.Add(name, value);
            }
        }

        public string BuildByVariable(string outputDirectory)
        {
            var Pattern = @"%([^%])*%";
            var rex = new Regex(Pattern, RegexOptions.IgnoreCase);
            var s = rex.Replace(outputDirectory, match =>
            {
                var key = match.Value.ToLower();
                return _variable.ContainsKey(key) ? _variable[key] : match.Value;
            }).Trim();
          
            return s;
        }
    }
}