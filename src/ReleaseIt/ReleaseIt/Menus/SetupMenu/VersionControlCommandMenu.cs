using System;
using ReleaseIt.VersionControls;

namespace ReleaseIt.Menus.SetupMenu
{
    public class VersionControlCommandMenu : Setting
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
            var isModify = !string.IsNullOrEmpty(command.UserName);

            Console.WriteLine(isModify
                ? "Please input UserName or press enter to use " + command.UserName
                : "Please input UserName:");
            var cmd = Console.ReadLine();
            if (!isModify && cmd != "")
            {
                command.UserName = cmd;
            }
        }

        private void SetPassword(VersionControl command)
        {
            var isModify = !string.IsNullOrEmpty(command.Password);
            while (true)
            {
                Console.WriteLine(isModify
                    ? "Input Password or press enter use before pwd:"
                    : "Please input Password,if not set,please press enter:");
                var cmd = Console.ReadLine();
                if (!isModify && cmd != "")
                {
                    command.Password = cmd;
                }
                break;
            }
        }

        private void SetUrl(VersionControl command)
        {
            var isModify = !string.IsNullOrEmpty(command.Url);
            do
            {
                if (isModify)
                    Console.WriteLine("Input new Url or press enter to use " + command.Url);
                else
                    Console.WriteLine("Please input url of version repository.");

                var cmd = Console.ReadLine();
                if (!isModify && cmd != "")
                {
                    command.Url = cmd;
                }
            } while (string.IsNullOrEmpty(command.Url));
        }

        private void SetWorkingCopy(VersionControl command)
        {
            var isModify = !string.IsNullOrEmpty(command.WorkingCopy);
            do
            {
                Console.WriteLine(isModify
                    ? "Input new workingcopy folder or Press Enter to use old " + command.WorkingCopy
                    : "Please input folder of workingcopy.");
                var cmd = Console.ReadLine();
                if (!string.IsNullOrEmpty(cmd))
                {
                    command.WorkingCopy = cmd;
                }
            } while (string.IsNullOrEmpty(command.WorkingCopy));
        }

        private void TryToRun(VersionControl command, CommandSet commandSet)
        {
            Console.WriteLine("Would you like to get source-code right now?(Y/N)");
            var readLine = Console.ReadLine();
            if (readLine != null)
            {
                var cmd = readLine.ToLower();
                if (cmd == "y")
                {
                    command.Invoke(new ExceuteResult("./"), commandSet);
                }
            }
            Console.WriteLine("Done!");
        }

        private VersionControl SetVcType()
        {
            while (true)
            {
                Console.WriteLine("1. SVN");
                Console.WriteLine("2. Git");
                var vcType = Console.ReadLine();
                if (vcType == null)
                    continue;

                switch (vcType.Trim())
                {
                    case "1":
                        return new Svn();
                        break;
                    case "2":
                        return new Git();
                        break;
                    default:
                        Console.WriteLine("please input 1 or 2.");
                        continue;
                }
            }
        }

        public override void Modify(int index, CommandSet commandSet, string resultPath)
        {
            var vc = (VersionControl)commandSet.Commands[index];
            SetUrl(vc);
            SetWorkingCopy(vc);
            SetUserName(vc);
            if (!string.IsNullOrEmpty(vc.UserName))
                SetPassword(vc);
            TryToRun(vc, commandSet);
        }

        protected override Command CreateCommand()
        {
            var command = SetVcType();

            return command;
        }

        public override string GetResultPath(Command command)
        {
            var vc = (VersionControl)command;
            return IoExtender.GetPath(this.WorkingFolder, vc.WorkingCopy);
        }

        public VersionControlCommandMenu(string workingFolder)
            : base(workingFolder)
        {
        }
    }
}