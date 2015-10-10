using System;

namespace ReleaseIt.ParameterBuilder
{
    internal class ParameterWithValue<T> : Parameter
    {
        private readonly Func<T, string> _convert;


        private readonly string _parameterName;
        private T _value;

        public ParameterWithValue(string name)
            : this(name, T => Convert.ToString(T))
        {
        }

        public ParameterWithValue(string name, T value)
            : this(name, T => Convert.ToString(T))
        {
            Value = value;
        }


        public ParameterWithValue(string name, Func<T, string> convert)
            : base(name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");
            if (convert == null) throw new ArgumentNullException("convert");
            _parameterName = name;
            _convert = convert;
            ValueSplitChar = ":";
        }

        public T Value
        {
            get { return _value; }
            set
            {
                if (!ReferenceEquals(_value, value))
                {
                    _value = value;
                }
            }
        }

        public string ValueSplitChar { get; set; }

        public override string Build()
        {
            return string.Format("{0}{2}{1}", _parameterName, _convert(Value), ValueSplitChar);
        }
    }
}