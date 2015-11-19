using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReleaseIt
{
    public class CommandCollection : KeyedCollection<String, ICommand>
    {
        public CommandCollection()
        {
            
        }
        public CommandCollection(IList<ICommand> commands)
            : base()
        {
            foreach (var command in commands)
            {
                this.Add(command);
            }
        }

        protected override string GetKeyForItem(ICommand item)
        {
            return item.Id;
        }
    }
}
