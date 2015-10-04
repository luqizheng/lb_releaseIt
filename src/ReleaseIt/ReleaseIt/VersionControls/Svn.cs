using System.IO;
using ReleaseIt.CommandFinders;
using ReleaseIt.ParameterBuilder;

namespace ReleaseIt.VersionControls
{
    public class Svn : Command
    {
        public Svn()
            : base(new SvnFinder())
        {
        }

        public string UserName { get; set; }
        public string Password { get; set; }
        public string Url { get; set; }

        public string WorkingFolder { get; set; }

        protected override ICmdParameter[] BuildParameters(string executeFolder)
        {
            return new ICmdParameter[]
            {
                new Parameter("", "checkout")
                {
                    HasSet = true
                },
                new Parameter("", Url)
                {
                    HasSet = true
                },
                new Parameter("", Path.Combine(executeFolder, WorkingFolder)),
                new ParameterWithValue<string>("username")
                {
                    Prefix = "--",
                    Value = UserName,
                    ValueSplitChar = " "
                },
                new ParameterWithValue<string>("password")
                {
                    Prefix = "--",
                    Value = Password,
                    ValueSplitChar = " "
                }
            };
        }

        protected override ExceuteResult CreateResult(string executeFolder)
        {
            return new ExceuteResult
            {
                ResultPath = Path.Combine(executeFolder, WorkingFolder)
            };
        }
    }
}