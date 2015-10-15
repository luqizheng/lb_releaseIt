using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReleaseIt.Commands
{
    public class EMailSetting
    {
        public string Server { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public string Content { get; set; }
        public string Subject { get; set; }
    }
    public class EmailCommand
    {
    }
}
