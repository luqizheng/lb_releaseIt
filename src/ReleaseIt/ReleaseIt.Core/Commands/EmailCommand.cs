using System.Net;
using System.Net.Mail;

namespace ReleaseIt.Commands
{
    public class EmailCommand : Command<EmailSetting>
    {
        protected override void InvokeByNewSetting(ExecuteSetting executeSetting, Setting setting)
        {
            var client = new SmtpClient(Setting.Host, Setting.Port) {EnableSsl = this.Setting.Ssl};
            if (!string.IsNullOrEmpty(Setting.UserName) && !string.IsNullOrEmpty(Setting.Password))
            {
                client.Credentials = new NetworkCredential(Setting.UserName, Setting.Password);
            }
            var mess = Setting.Create(executeSetting);
            client.Send(mess);
        }
    }
}