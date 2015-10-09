using ReleaseIt.CommandFinders;

namespace ReleaseIt.VersionControls
{
    public enum VersionControlType
    {
        Git,
        Svn
    }

    public abstract class VersionControl : Command
    {
        protected VersionControl(ICommandFinder finder) : base(finder)
        {
        }

        public string WorkingCopy { get; set; }

        public string Url { get; set; }

        public abstract string Branch { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }
    }
}