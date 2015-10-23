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
            Console.WriteLine("releaseIt [fileName] for run.");
            
            Console.WriteLine();

            Console.WriteLine("/skip:commandName;commandName for skip command");
            Console.WriteLine("/run:commandName;commandName run following command.");

            Console.WriteLine("/tags:tag1;tag2 for run");

            Console.WriteLine("/c [fileName] to create a template setting file.");
            Console.WriteLine("e.g. releaseIt setting.ini /t");

            return false;
        }
    }
}