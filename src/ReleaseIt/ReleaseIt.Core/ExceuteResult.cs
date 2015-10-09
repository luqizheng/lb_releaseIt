namespace ReleaseIt
{
    public class ExceuteResult
    {
        private string _workDirectory;
        private string _resultFolder;

        public ExceuteResult(string startFolder)
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
    }
}