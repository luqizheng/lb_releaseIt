using System;
using ReleaseIt.VersionControls;

namespace ReleaseIt
{

    public class SvnBuilder
    {
        private readonly Svn _svn;

        internal SvnBuilder(Svn svn)
        {
            _svn = svn;
        }

        public SvnBuilder User(string username)
        {
            if (username == null) throw new ArgumentNullException("username");
            _svn.UserName = username;
            return this;
        }

        public SvnBuilder Password(string password)
        {
            _svn.Password = password;
            return this;
        }

        public SvnBuilder Url(string url)
        {
            _svn.Url = url;
            return this;
        }
    }
}