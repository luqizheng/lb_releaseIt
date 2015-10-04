using ReleaseIt.VersionControls;

namespace ReleaseIt
{
    public class GitBuilder
    {
        private readonly Git _git;


        public GitBuilder(Git git)
        {
            _git = git;
        }

        public GitBuilder Url(string url)
        {
            _git.Url = url;
            return this;
        }

        public GitBuilder BrancheName(string branchName)
        {
            _git.Branch = branchName;
            return this;
        }

        public GitBuilder UserName(string userName, string password)
        {
            _git.UserName = userName;
            _git.Password = password;
            return this;
        }


        public GitBuilder WorkingCopy(string src)
        {
            _git.WorkingCopy = src;
            return this;
        }
    }
}