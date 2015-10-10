using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReleaseIt
{
    public class CopySetting
    {
        public string TargetPath { get; set; }
        public bool SaveLastCopyDate { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        /// <summary>
        ///     use /d to copy file.
        /// </summary>
        public DateTime? LastCopyDate { get; set; }

        public bool UserDateCompareCopy { get; set; }

    }
}
