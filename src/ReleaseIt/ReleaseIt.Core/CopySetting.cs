using System.ComponentModel;

namespace ReleaseIt
{
    [Description("copy setting. copy %result% to target path.")]
    public class CopySetting : Setting
    {
        private string _sourcePath;
        public string TargetPath { get; set; }

        public string SourcePath
        {
            get { return _sourcePath ?? "%result%"; }
            set { _sourcePath = value; }
        }

        public string UserName { get; set; }


        public string Password { get; set; }

        /// <summary>
        ///     use /d to copy file.
        /// </summary>
        public string LastCopyDate { get; set; }


        public bool UseDateCompareCopy { get; set; }
    }
}