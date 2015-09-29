using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReleaseIt.ParameterBuilder;

namespace ReleaseIt
{
    public class Command
    {
        private readonly string _name;

        protected Command(string name)
        {
            _name = name;
        }
        public string Name { get{return _name}}
        public void Invoke(string[] args)
        {

            var psi = new ProcessStartInfo(_name, String.Join(" ", args))
            {
                UseShellExecute = false
            };
            using (var process = Process.Start(psi))
            {
                process.WaitForExit();
                if (process.ExitCode != 0)
                    throw new Exception(string.Format(CultureInfo.InvariantCulture, "{0} returned a non-zero exit code",
                        Path.GetFileName(psi.FileName)));
            }
        }
    }
}
