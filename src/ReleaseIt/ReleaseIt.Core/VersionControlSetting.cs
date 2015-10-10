namespace ReleaseIt
{
    public enum VersionControlType
    {
        Git,
        Svn
    }


    public class VersionControlSetting
    {
        public VersionControlSetting()
        {

        }

        public VersionControlType VersionControlType { set; get; }
        public string WorkingCopy { get; set; }

        public string Url { get; set; }

        public string Branch { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }
    }

    public static class VcBuilderExtender
    {
        public static VCBuilder Svn(this CommandSet set)
        {
            var result = new VersionControlSetting()
            {
                VersionControlType = VersionControlType.Svn
            };
            set.Add(result);
            return new VCBuilder(result);
        }

        public static VCBuilder Git(this CommandSet set)
        {
            var result = new VersionControlSetting()
            {
                VersionControlType = VersionControlType.Git
            };
            set.Add(result);
            return new VCBuilder(result);
        }
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
            this._setting.Url = url;;
            return this;
        }

        public VCBuilder Auth(string username, string password)
        {
            this._setting.UserName = username;
            this._setting.Password = password;
            return this;
        }

        public VCBuilder WorkingCopy(string workingCopy)
        {
            this._setting.WorkingCopy = workingCopy;
            return this;
        }

    }
}