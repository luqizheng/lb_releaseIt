using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace ReleaseIt
{
    [Description("output")]
    public class VersionControlSetting : Setting
    {
        private string _url;
        [DataMember]
        public VersionControlType VersionControlType { set; get; }
        [DataMember]
        public string WorkingCopy { get; set; }
        [DataMember]
        public string Url
        {
            get { return _url; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException("value", "Url should not be null.");
                }
                _url = value.Trim();
            }
        }
        [DataMember]
        public string Branch { get; set; }
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string Password { get; set; }
    }

    public class VCBuilder
    {
        private readonly VersionControlSetting _setting;

        public VCBuilder(VersionControlSetting setting)
        {
            _setting = setting;
        }

        public VCBuilder Url(string url)
        {
            _setting.Url = url;
            ;
            return this;
        }

        public VCBuilder Auth(string username, string password)
        {
            _setting.UserName = username;
            _setting.Password = password;
            return this;
        }

        public VCBuilder WorkingCopy(string workingCopy)
        {
            _setting.WorkingCopy = workingCopy;
            return this;
        }

        public VCBuilder Name(string commandName)
        {
            _setting.Name = commandName;
            return this;
        }
    }


    public enum VersionControlType
    {
        Git,
        Svn
    }

}