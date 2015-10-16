using System;
using System.Collections.Generic;
using System.IO;
using ReleaseIt.Arguments;
using ReleaseIt.IniStore;

namespace ReleaseIt
{
    internal class Program
    {
        //private static readonly JsonSerializerSettings setting = new JsonSerializerSettings
        //{
        //    Formatting = Formatting.Indented,
        //    TypeNameHandling = TypeNameHandling.Auto,
        //    TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple
        //};

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
            var fileInfo = new FileInfo(args[0]);
            var executeSetting = new ExecuteSetting(fileInfo.Directory.FullName);
            executeSetting.ForWidnow();
            var commandSet = new CommandSet(executeSetting);
            var factory = new ArgumentFactory();
            var invoked = factory.Handle(list, commandSet, fileInfo.FullName);

            if (invoked)
                Run(commandSet, fileInfo);
        }

        private static void Run(CommandSet commandSet, FileInfo fullName)
        {
            var manager = new SettingManager();
            manager.ReadSetting(commandSet, fullName.FullName);
            commandSet.Invoke();
        }
    }
}