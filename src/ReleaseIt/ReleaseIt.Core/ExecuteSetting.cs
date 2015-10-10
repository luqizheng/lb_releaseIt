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
        /// ִ����Ϻ��λ��
        /// </summary>
        public string ResultFolder
        {
            get { return _resultFolder ?? StartFolder; }
            set { _resultFolder = value; }
        }

        /// <summary>
        /// �ʼִ�е�λ��.
        /// </summary>
        public string StartFolder { get; private set; }
        /// <summary>
        /// ִ�е����ļ�,��ô�������ֵ. ��Msbuild
        /// </summary>
        public string ExecuteFile { get; set; }

        /// <summary>
        /// ����Ŀ¼
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