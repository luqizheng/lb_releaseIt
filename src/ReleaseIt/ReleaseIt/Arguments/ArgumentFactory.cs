using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ReleaseIt.Arguments
{
    public class ArgumentFactory
    {
        private readonly Dictionary<string, ArgumentHandler> _creator = new Dictionary<string, ArgumentHandler>();

        public ArgumentFactory()
        {
            Add(new SkipHandler());
            Add(new SkipTagHandler());
            Add(new ShowHelpArgumentHandler());
            Add(new SaveTemplateHandler());
            Add(new RunTags());
            Add(new RunHandler());
            Add(new ProcessNoLog());
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
                var paramName = Regex.Match(key, "[A-z0-9]*").Value;
                if (_creator.ContainsKey(paramName))
                {
                    var goOn = _creator[paramName].Handle(set, fileName, key);
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