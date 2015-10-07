using System;
using ReleaseIt.VersionControls;

namespace ReleaseIt.Settings
{
    public class VersionControlSetting : SetupSetting
    {
        public override string Key
        {
            get { return "v"; }
        }

        public override string Description
        {
            get { return "set version contorl system setting."; }
        }


        private void SetUserName(VersionControl command)
        {
            Console.WriteLine("Please input UserName:");
            var cmd = Console.ReadLine();
            command.UserName = cmd;
        }

        private void SetPassword(VersionControl command)
        {
            Console.WriteLine("Please input Password,if not set,please enter:");
            var cmd = Console.ReadLine();
            command.Password = cmd;
        }

        private void SetUrl(VersionControl command)
        {
            Console.WriteLine("Please input url of version repository.");
            var cmd = Console.ReadLine();
            command.Url = cmd;
        }

        private void SetWorkingCopy(VersionControl command)
        {
            Console.WriteLine("Please input folder of workingcopy.");
            var cmd = Console.ReadLine();
            if (!string.IsNullOrEmpty(cmd))
            {
                command.WorkingCopy = cmd;
            }
        }

        private void TryToRun(VersionControl command, CommandFactory commandFactory)
        {
            Console.WriteLine("Would  you like to get source-cdoe?");
            var cmd = Console.ReadLine().ToLower();
            if (cmd == "y")
            {
                command.Invoke("./", commandFactory);
            }
        }

        private VersionControl SetVcType()
        {
            while (true)
            {
                Console.WriteLine("1. SVN");
                Console.WriteLine("2. Git");
                var a = Console.ReadLine();
                var vcType = Convert.ToInt32(a);
                switch (vcType)
                {
                    case 1:
                        return new Svn();
                        break;
                    case 2:
                        return new Git();
                        break;
                    default:
                        Console.WriteLine("please input 1 or 2.");
                        continue;
                }
            }
        }

        protected override Command CreateCommand(CommandFactory commandFactory)
        {
            var command = SetVcType();
            SetUrl(command);
            SetWorkingCopy(command);
            SetUserName(command);
            if (!string.IsNullOrEmpty(command.UserName))
                SetPassword(command);
            TryToRun(command, commandFactory);
            return command;
        }
    }
}