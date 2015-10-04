using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReleaseIt
{
    public class ParameterException : ApplicationException
    {
        public ParameterException(string parameterName, string message)
            : base(String.Format("{0} has error, {1}", parameterName, message))
        {

        }
    }
}
