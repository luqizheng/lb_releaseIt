using System.Net;
using System.Net.Mail;

namespace ReleaseIt.Commands
{
    public class EmailCommand : Command<SmtpEmailSetting>
    {
        public EmailCommand(SmtpEmailSetting setting)
            : base(setting)
        {
        }

        protected override void InvokeByNewSetting(ExecuteSetting executeSetting, Setting setting)
        {
            var client = new SmtpClient(Setting.Host, Setting.Port) { EnableSsl = this.Setting.EnableSsl };
            if (!string.IsNullOrEmpty(Setting.UserName) && !string.IsNullOrEmpty(Setting.Password))
            {
                client.Credentials = new NetworkCredential(Setting.UserName, Setting.Password);
            }
            var mess = Setting.Create(executeSetting);
            client.Send(mess);
        }

        public override ICommand Clone()
        {
            return new EmailCommand(Setting.Clone() as SmtpEmailSetting);
        }
    }
}