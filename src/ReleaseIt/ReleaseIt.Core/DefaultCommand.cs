using ReleaseIt.WindowCommand.CommandFinders;

namespace ReleaseIt.Publish
{
    public class DefaultCommand : ICommandFinder
    {
        private readonly string _commandLine;
        private readonly string _name;

        public DefaultCommand(string name, string commandLine)
        {
            _name = name;
            _commandLine = commandLine;
        }

        public string Name
        {
            get { return _name; }
        }

        public string FindCmd()
        {
            return _commandLine;
        }
    }
}