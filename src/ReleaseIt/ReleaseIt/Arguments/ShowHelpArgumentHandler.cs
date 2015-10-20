using System;

namespace ReleaseIt.Arguments
{
    public class ShowHelpArgumentHandler : ArgumentHandler
    {
        public override string Key
        {
            get { return "help"; }
        }

        public override bool Handle(CommandSet set, string fileName, string argument)
        {
            Console.WriteLine("releaseIt [fileName]  for run.");
            Console.WriteLine("/t releaseIt setting.ini");
            Console.WriteLine();

            Console.WriteLine("/s:nameOfCommand;nameOfCommand for skip command");
            Console.WriteLine("/r:nameOfCommand;nameOfCommand run following command.");

            Console.WriteLine("/t:tag1;tag2 for run");

            Console.WriteLine("/c [fileName] to create a template setting file.");
            Console.WriteLine("e.g. releaseIt setting.ini /t");

            return false;
        }
    }
}