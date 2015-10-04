using ReleaseIt.VersionControls;

namespace ReleaseIt
{
    public class VersionControlerBuilder
    {
        private readonly VersionControl _git;


        public VersionControlerBuilder(VersionControl git)
        {
            _git = git;
        }

        public VersionControlerBuilder Url(string url)
        {
            _git.Url = url;
            return this;
        }

        public VersionControlerBuilder BrancheName(string branchName)
        {
            _git.Branch = branchName;
            return this;
        }

        public VersionControlerBuilder UserName(string userName, string password)
        {
            _git.UserName = userName;
            _git.Password = password;
            return this;
        }


        public VersionControlerBuilder WorkingCopy(string src)
        {
            _git.WorkingCopy = src;
            return this;
        }
    }
}