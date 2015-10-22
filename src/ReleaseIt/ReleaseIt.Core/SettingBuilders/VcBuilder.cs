using System;
using ReleaseIt.Commands;

namespace ReleaseIt.SettingBuilders
{
    public class VcBuilder : SettingBuilderBase<VersionControlSetting, VcBuilder>
    {
        public VcBuilder(VersionControlSetting setting)
            : base(setting)
        {
        }

        public VcBuilder Url(string url)
        {
            if (url == null) throw new ArgumentNullException("url");
            _setting.Url = url;
            return this;
        }

        public VcBuilder Auth(string username, string password)
        {
            if (string.IsNullOrEmpty(username)) throw new ArgumentNullException("username");
            if (string.IsNullOrEmpty(password)) throw new ArgumentNullException("password");
            _setting.UserName = username;
            _setting.Password = password;
            return this;
        }

        public VcBuilder WorkingCopy(string workingCopy)
        {
            _setting.WorkingCopy = workingCopy;
            return this;
        }
    }


    public class SmtpEmailBuilder : SettingBuilderBase<SmtpEmailSetting, SmtpEmailBuilder>
    {
        /*一、POP3收邮件：
POP3: 110
POP3 SSL: 995
二、IMAP收邮件：
IMAP: 143
IMAP SSL: 993
三、SMTP发邮件：
SMTP: 25
SMTP SSL: 465
SMTP TLS: 587
         * */


        public SmtpEmailBuilder(SmtpEmailSetting emailsetting)
            : base(emailsetting)
        {
        }


        public SmtpEmailBuilder Auth(string username, string password)
        {
            _setting.Password = password;
            _setting.UserName = username;
            return this;
        }

        public SmtpEmailBuilder SmtpServer(string server)
        {
            _setting.Host = server;
            return this;
        }

        public SmtpEmailBuilder Port(int port)
        {
            _setting.Port = port;
            return this;
        }

        public SmtpEmailBuilder EnabledSsl()
        {
            _setting.EnableSsl = true;
            return this;
        }
    }
}