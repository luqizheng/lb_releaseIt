using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ReleaseIt
{
    public class ExecuteSetting : ICloneable
    {
        private const string VAR_RESULT = "%result%";
        private const string VAR_Date = "%date%";
        private const string VAR_TIME = "%time%";
        private readonly Dictionary<string, string> _variable = new Dictionary<string, string>();
        private string _startFolder;
        private string _workDirectory;

        private ExecuteSetting(ExecuteSetting parent)
        {
            Parent = parent;
            Top = parent.Top ?? parent;
        }

        public ExecuteSetting(string startFolder)
        {
            if (startFolder == null) throw new ArgumentNullException("startFolder");
            _startFolder = startFolder;
            AddVariable(VAR_RESULT, startFolder);
            AddVariable(VAR_Date, DateTime.Now.ToString("yyyy-MM-dd"));
            AddVariable(VAR_TIME, DateTime.Now.ToString("HH:mm:ss"));
        }

        internal ExecuteSetting Top { get; set; }
        internal ExecuteSetting Parent { get; set; }
        public ConfigurationSetting Setting { get; set; }

        /// <summary>
        ///     after command execute %result%
        /// </summary>
        public string ResultFolder
        {
            get
            {
                if (_variable.ContainsKey(VAR_RESULT))
                    return GetVaribale(VAR_RESULT);
                return StartFolder;
            }
            set { AddVariable(VAR_RESULT, value); }
        }

        /// <summary>
        ///     start folder
        /// </summary>
        public string StartFolder
        {
            get { return Top != null ? Top.StartFolder : _startFolder; }
            private set
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                _startFolder = value;
            }
        }


        /// <summary>
        ///     ¹¤×÷Ä¿Â¼
        /// </summary>
        public string WorkDirectory
        {
            get { return _workDirectory ?? StartFolder; }
            set { _workDirectory = value; }
        }

        /// <summary>
        ///     Command is done or not.
        /// </summary>
        internal bool Done { get; set; }


        public object Clone()
        {
            var result = new ExecuteSetting(this)
            {
                WorkDirectory = WorkDirectory,
                ResultFolder = ResultFolder,
                Setting = Setting
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