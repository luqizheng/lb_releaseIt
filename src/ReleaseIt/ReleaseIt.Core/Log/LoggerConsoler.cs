using System;

namespace ReleaseIt.Log
{
    public class LoggerConsoler : ILog
    {
        public LoggerConsoler()
        {
            InfoColor = ConsoleColor.DarkGreen;
            WarnColor = ConsoleColor.Red;
        }

        public ConsoleColor InfoColor { get; set; }
        public ConsoleColor WarnColor { get; set; }

        public void Info(string str)
        {
            var defColor = Console.ForegroundColor;
            Console.ForegroundColor = InfoColor;
            Console.WriteLine(str);
            Console.ForegroundColor = defColor;
        }

        public void Error(string str)
        {
            var defColor = Console.ForegroundColor;
            Console.ForegroundColor = WarnColor;
            Console.ForegroundColor = defColor;
        }
    }
}