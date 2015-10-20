using System.Collections.Generic;

namespace ReleaseIt.Arguments
{
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
                var paramName = key.Substring(0, 1);
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