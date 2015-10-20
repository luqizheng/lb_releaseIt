using System.Net;
using System.Net.Mail;

namespace ReleaseIt.Commands
{
    public class EmailCommand : Command<EmailSetting>
    {
        public override void Invoke(ExecuteSetting executeSetting)
        {
            var client = new SmtpClient(Setting.Host, Setting.Port);
            if (!string.IsNullOrEmpty(Setting.UserName) && !string.IsNullOrEmpty(Setting.Password))
            {
                client.Credentials = new NetworkCredential(Setting.UserName, Setting.Password);
            }
            var mess = Setting.Create(executeSetting);
            client.Send(mess);
        }
    }
}