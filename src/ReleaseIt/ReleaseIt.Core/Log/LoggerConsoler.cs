using System;

namespace ReleaseIt.Log
{
    public class LoggerConsoler : ILog
    {
        public void WriteLine(string str)
        {
            Console.WriteLine(str);
        }

        public void Write(string str)
        {
            Console.Write(str);
        }
    }
}