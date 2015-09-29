using System;
using System.Runtime.CompilerServices;

namespace ReleaseIt.ParameterBuilder
{
    public class CmdParameter<T>
    {
        private readonly Func<T, string> _convert;

        private readonly T _notSpecialValue;
        private readonly string _parameterName;

        public CmdParameter(string name, T notspecialValue)
            : this(name, notspecialValue, T => Convert.ToString(T))
        {

        }

        public CmdParameter(string name, T notSpecialValue, Func<T, string> convert)
        {
            if (String.IsNullOrEmpty(name)) 
                throw new ArgumentNullException("name");
            if (convert == null) throw new ArgumentNullException("convert");
            _parameterName = name;
            _notSpecialValue = notSpecialValue;
            _convert = convert;
        }

        public T Value { get; set; }

        public string Build()
        {
            if (Equals(Value, _notSpecialValue))
                return "";

            return string.Format("{0}{1}", _parameterName, _convert(Value));
        }
    }
}