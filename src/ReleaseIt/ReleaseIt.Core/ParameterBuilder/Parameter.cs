namespace ReleaseIt.ParameterBuilder
{
    internal class Parameter : ICmdParameter
    {
        public Parameter(string name)
            : this("/", name)
        {
        }

        public Parameter(string prefix, string name)
        {
            Prefix = prefix;
            Name = name;
        }

        public string Prefix { get; set; }
        
        public string Name { get; set; }

        public virtual string Build()
        {
            return string.Format("{0}{1}", Prefix, Name);
        }


        public override string ToString()
        {
            return Build();
        }
    }
}