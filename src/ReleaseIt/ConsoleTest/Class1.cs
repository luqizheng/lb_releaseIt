using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTest
{
    public class Class1
    {
        private System.IO.DirectoryInfo _info;
        public Class1(string folder)
        {
            _info = new System.IO.DirectoryInfo(folder);

        }

        public void MakeFromTemplate(string templatefile)
        {
            foreach(var file in _info.GetFiles("*.csproj", SearchOption.AllDirectories))
            {
                Console.WriteLine(file);
            }

        }
    }
}
