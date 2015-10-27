using ReleaseIt.Log;

namespace ReleaseIt.Arguments
{
    public class ProcessNoLog : ArgumentHandler
    {
        public override string Key
        {
            get { return "nolog"; }
        }

        public override bool Handle(CommandSet set, string fileName, string argument)
        {
            set.Configruration.ProcessLogger = new EmptyLogger();
            return true;
        }
    }
}