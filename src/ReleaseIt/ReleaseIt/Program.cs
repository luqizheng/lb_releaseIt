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

        private static void Main(string[] args)
        {
            Console.Clear();
            Console.WriteLine("LB_Release 1.0");
            Console.WriteLine();
            Console.WriteLine();
            string file = null;
            var list = new List<string>();
            foreach (var arg in args)
            {
                if (arg.StartsWith("/"))
                {
                    list.Add(arg.Substring(1));
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

            if (file == null)
            {
                Console.WriteLine("Please input setting file, or use /h to show help.");
                return;
            }
            _fileInfo = new FileInfo(file);
            var executeSetting = new ExecuteSetting(Environment.CurrentDirectory);
            Console.WriteLine("Current Directory:" + executeSetting.StartFolder);
            executeSetting.ForWidnow();
            var commandSet = new CommandSet(executeSetting);
            var factory = new ArgumentFactory();
            var invoked = factory.Handle(list, commandSet, _fileInfo.FullName);

            if (invoked)
                Run(commandSet, _fileInfo);
        }

        public static IniFile ExistFile { get; private set; }

        private static void Run(CommandSet commandSet, FileInfo fullName)
        {

            var manager = new SettingManager();
            ExistFile = manager.ReadSetting(commandSet, fullName.FullName);
            commandSet.OnCommandSettingChanged += commandSet_OnCommandSettingChanged;
            commandSet.Invoke();
        }

        static void commandSet_OnCommandSettingChanged(object sender, EventArgs e)
        {
            var manager = new SettingManager();
            manager.Save(ExistFile, (CommandSet)sender, _fileInfo.FullName);
        }
    }
}