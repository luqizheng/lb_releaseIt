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
        ///     ִ����Ϻ��λ��
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
        ///     �ʼִ�е�λ��.
        /// </summary>
        public string StartFolder { get; private set; }

        /// <summary>
        ///     ִ�е����ļ�,��ô�������ֵ. ��Msbuild
        /// </summary>
        public string ExecuteFile { get; set; }

        /// <summary>
        ///     ����Ŀ¼
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

        public string GetVaribale(string name)
        {
            name = name.ToLower();
            if (!_variable.ContainsKey(name))
                throw new ArgumentOutOfRangeException("name", "Can't find variable which named " + name);
            return _variable[name];
        }
        public string BuildByVariable(string outputDirectory)
        {
            return BuildByVariable(outputDirectory, false);
        }

        public string BuildByVariable(string outputDirectory, bool forPathWrapper)
        {
            var Pattern = @"%([^%])*%";
            var rex = new Regex(Pattern, RegexOptions.IgnoreCase);
            var path = rex.Replace(outputDirectory, match =>
            {
                var key = match.Value.ToLower();
                if (!_variable.ContainsKey(key))
                {
                    throw new ApplicationException("Can't not find variable " + key);
                }
                return _variable[key];
            }).Trim();
            if (forPathWrapper)
            {
                return IoExtender.WrapperPath(path);
            }
            return path;
        }
    }
}