using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReleaseIt.CommandFinders;
using ReleaseIt.ParameterBuilder;

namespace ReleaseIt.VersionControls
{
    class Git : Command
    {
        public Git()
            : base(new GitFinder())
        {
        }

        protected override ICmdParameter[] BuildParameters(string executeFolder)
        {
            return new[]
            {
                new Parameter("", "pull"),
            };
        }

        protected override ExceuteResult CreateResult(string executeFolder)
        {
            return new ExceuteResult()
            {
                ResultPath = executeFolder
            };
        }
    }
}
