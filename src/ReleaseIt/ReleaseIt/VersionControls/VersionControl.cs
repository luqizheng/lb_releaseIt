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
    public class VersionControl
    {
        public string CmdPath { get; set; }


    }


}
