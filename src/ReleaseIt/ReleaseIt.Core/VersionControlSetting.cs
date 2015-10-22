using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace ReleaseIt
{
    public class GitSetting : VersionControlSetting
    {
    }

    public class SvnSetting : VersionControlSetting
    {
    }

    [Description("output")]
    public abstract class VersionControlSetting : Setting
    {
        private string _url;

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

}