using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using ReleaseIt.CommandFinders;

namespace ReleaseIt.VersionControls
{
    public enum VersionControlType
    {
        Git, Svn,
    }
    public abstract class VersionControl:Command
    {
        
        public string WorkingCopy { get; set; }

        public string Url { get; set; }

        public abstract string Branch { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        protected VersionControl(ICommandFinder finder) : base(finder)
        {
        }
    }


}
