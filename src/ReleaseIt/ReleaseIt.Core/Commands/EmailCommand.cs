using System.Net.Mail;

namespace ReleaseIt.Commands
{
    public class EmailCommand : Command<EmailSetting>
    {
        public override void Invoke(ExecuteSetting executeSetting)
        {
            var client = new SmtpClient(Setting.Host, Setting.Port);
            var mess = Setting.Create(executeSetting);
            client.Send(mess);
        }
    }
}