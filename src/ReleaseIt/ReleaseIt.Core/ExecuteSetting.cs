using System;

namespace ReleaseIt
{
    public class ExecuteSetting:ICloneable
    {
        private string _workDirectory;
        private string _resultFolder;

        public ExecuteSetting(string startFolder)
        {
            this.StartFolder = startFolder;
        }

        /// <summary>
        /// 执行完毕后的位置
        /// </summary>
        public string ResultFolder
        {
            get { return _resultFolder ?? StartFolder; }
            set { _resultFolder = value; }
        }

        /// <summary>
        /// 最开始执行的位置.
        /// </summary>
        public string StartFolder { get; private set; }
        /// <summary>
        /// 执行的是文件,那么这个就有值. 如Msbuild
        /// </summary>
        public string ExecuteFile { get; set; }

        /// <summary>
        /// 工作目录
        /// </summary>
        public string WorkDirectory
        {
            get { return _workDirectory ?? StartFolder; }
            set { _workDirectory = value; }
        }


        public object Clone()
        {
            return new ExecuteSetting(this.StartFolder)
            {
                ExecuteFile = ExecuteFile,
                WorkDirectory = WorkDirectory,
                ResultFolder = ResultFolder,

            };
        }
    }
}