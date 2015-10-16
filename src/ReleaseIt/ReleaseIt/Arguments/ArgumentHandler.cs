using System.Collections.Generic;

namespace ReleaseIt.Arguments
{
    public abstract class ArgumentHandler
    {
        public abstract string Key { get; }

        public abstract bool Handle(CommandSet set, string fileName, string argument);
    }

    public class ArgumentFactory
    {
        private readonly Dictionary<string, ArgumentHandler> _creator = new Dictionary<string, ArgumentHandler>();

        public ArgumentFactory()
        {
            Add(new SkipHandler());
            Add(new ShowHelpArgumentHandler());
            Add(new SaveTemplateHandler());
        }

        private void Add(ArgumentHandler handler)
        {
            _creator.Add(handler.Key, handler);
        }

        /// <summary>
        /// </summary>
        /// <param name="keies"></param>
        /// <param name="set"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public bool Handle(IEnumerable<string> keies, CommandSet set, string fileName)
        {
            foreach (var key in keies)
            {
                if (_creator.ContainsKey(key.Substring(0, 1)))
                {
                    var goOn = _creator[key].Handle(set, fileName, key);
                    if (!goOn)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}