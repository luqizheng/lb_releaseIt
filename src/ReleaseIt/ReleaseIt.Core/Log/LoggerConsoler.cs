using System;

namespace ReleaseIt.Log
{
    public class LoggerConsoler : ILog
    {
        public LoggerConsoler()
        {
            InfoColor = ConsoleColor.DarkGreen;
        }

        public ConsoleColor InfoColor { get; set; }
        public ConsoleColor WarnColor { get; set; }

        public void Info(string str)
        {
            Console.ForegroundColor = InfoColor;
            Console.WriteLine(str);
        }

        public void Error(string str)
        {
            Console.ForegroundColor = WarnColor;
            Console.Write(str);
        }
    }

    public class EmptyLogger : ILog
    {
        public void Info(string str)
        {
        }

        public void Error(string str)
        {
        }
    }
}