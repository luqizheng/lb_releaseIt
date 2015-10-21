using System.Net.Mail;

namespace ReleaseIt.Commands
{
    public class EmailSetting : Setting
    {
        private string _form;
        public string Host { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public string Content { get; set; }
        public string Subject { get; set; }

        public string FromAddr
        {
            get { return _form ?? UserName; }
            set { _form = value; }
        }

        public bool Ssl { get; set; }


        public string ToAddr { get; set; }

        public MailMessage Create(ExecuteSetting executeSetting)
        {
            var message = new MailMessage();
            message.Subject = executeSetting.BuildByVariable(Subject);
            message.Body = executeSetting.BuildByVariable(Content);
            message.From = new MailAddress(FromAddr);
            foreach (var toItem in ToAddr.Split(';'))
            {
                message.To.Add(new MailAddress(toItem));
            }

            return message;
        }
    }
}