using System;
using System.Collections.Generic;
using ReleaseIt.MsBuilds;
using ReleaseIt.Publish;
using ReleaseIt.VersionControls;

namespace ReleaseIt.Menus.SetupMenu
{
    public class SetOrder : MenusList
    {
        private readonly Dictionary<Type, Action<int, CommandSet, string>> _actions
            = new Dictionary<Type, Action<int, CommandSet, string>>();

        public SetOrder(string workingFolder)
            : base(workingFolder)
        {
            SetupCommandBuildMenu();
        }

        public override string Key
        {
            get { return "o"; }
        }

        public override string Description
        {
            get { return "Set CofnigFile execute order."; }
        }

        private void SetupCommandBuildMenu()
        {
            _actions.Add(typeof (MsBuild), (cmd, cmdFactory, resultPath) =>
            {
                var setting = new MsBuildCommandMenu(WorkingFolder);
                setting.Modify(cmd, cmdFactory, resultPath);
            });

            _actions.Add(typeof (Svn), (cmd, cmdFactory, resultPath) =>
            {
                var setting = new VersionControlCommandMenu(WorkingFolder);
                setting.Modify(cmd, cmdFactory, resultPath);
            });
            _actions.Add(typeof (Git), (cmd, cmdFactory, resultPath) =>
            {
                var setting = new VersionControlCommandMenu(WorkingFolder);
                setting.Modify(cmd, cmdFactory, resultPath);
            });

            _actions.Add(typeof (XCopy), (cmd, cmdFactory, resultPath) =>
            {
                var setting = new XCopySetting(WorkingFolder);
                setting.Modify(cmd, cmdFactory, resultPath);
            });
        }

        public override Menu Do(CommandSet commandSet, string resultPath)
        {
            Console.Clear();
            Console.WriteLine("Coimmand: <command_key><record index>");
            Console.WriteLine("e:Edit, d:Delete, Move-up:u,Move-down:d");
            Console.WriteLine("e.g. e0 for edit the first record");
            Console.WriteLine();
            Console.WriteLine("Command list:");
            Console.WriteLine();
            for (var i = 0; i < commandSet.Commands.Count; i++)
            {
                Console.WriteLine("{0} - {1}", i, commandSet.Commands[i]);
            }
            while (true)
            {
                var input = Console.ReadLine();
                if (input == null)
                {
                    Console.WriteLine("Error Command!");
                    continue;
                }
                if (input.ToLower() == "exit")
                    break;
                try
                {
                    HandleCommand(input, commandSet, resultPath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error Command!");
                }
            }

            return Parent;
        }

        private void HandleCommand(string input, CommandSet commandSet, string resultPath)
        {
            var cmd = input.Substring(0, 1);
            var index = Convert.ToInt32(input.Substring(1));

            if (cmd == "e")
            {
                Modify(index, commandSet, resultPath);
                return;
            }

            if (cmd == "u")
            {
                Up(index, commandSet);
            }
            if (cmd == "d")
            {
                var target = index++;
                if (target != 0)
                {
                    Down(index, commandSet);
                }
            }
        }

        private void Down(int index, CommandSet commandSet)
        {
        }

        private void Up(int commandIndex, CommandSet commandSet)
        {
        }

        private void Modify(int command, CommandSet commandSet, string resultPath)
        {
            var type = command.GetType();
            _actions[type](command, commandSet, resultPath);
        }
    }
}