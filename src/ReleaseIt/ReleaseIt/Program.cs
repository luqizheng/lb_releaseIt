using System;
using System.Collections.Generic;
using System.IO;
using ReleaseIt.Arguments;
using ReleaseIt.IniStore;

namespace ReleaseIt
{
    internal class Program
    {
        private static FileInfo _fileInfo;

        public static IniFile ExistFile { get; private set; }

        private static void Main(string[] args)
        {
            Console.Clear();
            Console.WriteLine("LB_Release 1.0");
            Console.WriteLine();
            string file = null;
            var list = new List<string>();
            foreach (var arg in args)
            {
                if (arg.StartsWith("/") || arg.StartsWith("-"))
                {
                    list.Add(arg.Substring(1).ToLower());
                    continue;
                }
                if (file == null)
                {
                    file = arg;
                    continue;
                }
                Console.WriteLine("Error Argument " + file + " please use /h to show help.");
                return;
            }
            if (list.Contains((new ShowHelpArgumentHandler()).Key))
            {
                (new ShowHelpArgumentHandler()).Handle(null, null, null);
                return;
            }
            if (file == null && list.Count == 0)
            {
                Console.WriteLine("Please input setting file, or use /h to show help.");
                return;
            }

            _fileInfo = new FileInfo(file);
            var executeSetting = new ExecuteSetting(Environment.CurrentDirectory);
            executeSetting.ForWidnow();
            var commandSet = new CommandSet(executeSetting);
            var factory = new ArgumentFactory();
            var invoked = factory.Handle(list, commandSet, _fileInfo.FullName);

            if (invoked)
            {
                Run(commandSet, _fileInfo);
            }

            Console.Write("Press any key to exit");
            Console.Read();
        }

        private static void Run(CommandSet commandSet, FileInfo fullName)
        {

            if (!fullName.Exists)
            {
                Console.WriteLine(fullName.Name + " not found.");
                return;
            }
            var manager = new SettingManager();
            ExistFile = manager.ReadSetting(commandSet, fullName.FullName);
            commandSet.OnCommandSettingChanged += commandSet_OnCommandSettingChanged;
            commandSet.Invoke();


        }

        private static void commandSet_OnCommandSettingChanged(object sender, EventArgs e)
        {
            var manager = new SettingManager();
            manager.Save(ExistFile, (CommandSet)sender, _fileInfo.FullName);
        }
    }
}