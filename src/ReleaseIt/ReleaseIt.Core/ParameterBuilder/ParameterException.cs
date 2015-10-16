using System;

namespace ReleaseIt.ParameterBuilder
{
    public class ParameterException : ApplicationException
    {
        public ParameterException(string parameterName, string message)
            : base(string.Format("{0} has error, {1}", parameterName, message))
        {
        }
    }
}