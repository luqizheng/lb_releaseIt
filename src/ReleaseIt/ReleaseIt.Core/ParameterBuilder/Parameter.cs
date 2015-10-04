using System;

namespace ReleaseIt.ParameterBuilder
{
    public class Parameter : ICmdParameter
    {
        public string Prefix { get; set; }

        public Parameter(string name)
            : this("/", name)
        {

        }
        public Parameter(string prefix, string name)
        {
            Prefix = prefix;
            this.Name = name;
        }
        public virtual bool HasSet { get; set; }
        public string Name { get; set; }

        public virtual string Build()
        {
            return String.Format("{0}{1}", Prefix, Name);
        }


        public override string ToString()
        {
            return Build();
        }
    }
}