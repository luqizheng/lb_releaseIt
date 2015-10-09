using System;
using System.IO;

namespace ReleaseIt
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.Clear();
            Console.WriteLine("LB_Release 1.0");
            Console.WriteLine();
            Console.WriteLine();
            var fileName = "";
            var parameter = "";
            for (var index = 0; index < args.Length; index++)
            {
                var arg = args[index];
                if (arg.StartsWith("/"))
                {
                    if (arg == "/s")
                    {
                        var nextFileIndex = ++index;
                        if (args.Length > nextFileIndex)
                            ShowSetting(args[nextFileIndex]);
                        else
                            Console.WriteLine("Please use /s filename to start ");
                        return;
                    }
                    ShowHelp();
                    return;
                }
            }
            var fileInfo = new FileInfo(args[0]);
            CommandSet.CreateFrom(args[0]).Invoke(fileInfo.Directory.FullName);
        }

        private static void ShowSetting(string fileName)
        {
            //CreateTemplate

            var command = CommandSet.Create();
            command.Svn().Url("http://svn.address.com/trunk").UserName("username", "password")
                .WorkingCopy("workongfolder");
            command.MsBuild(true).Release().ProjectPath("/mypathforcsproj").CopyTo("PublishPath");

            command.CopyTo("publish");

            command.Save(fileName);
        }

        /*
        private static void ShowSetting_bak(string fileName)
        {
            var exists = File.Exists(fileName);
            if (!exists)
            {
                Console.WriteLine(fileName + " is no exist. so will create a new one.");
            }
            var commands =
                exists
                    ? CommandSet.CreateFrom(fileName)
                    : new CommandSet();

            var directoryInfo = new FileInfo(fileName).Directory;
            if (directoryInfo != null)
                Current = new Top(!exists, (directoryInfo.FullName));
            else
            {
                throw new ArgumentOutOfRangeException("fileName", fileName + "Not exist.");
            }
            while (true)
            {
                Current = Current.Do(commands, directoryInfo.FullName);
                if (Current == null)
                    break;
            }
            Console.WriteLine();
            Console.WriteLine(Menu.ExitSaveOrNot.Description);
            var cmd = Console.ReadLine();
            if (cmd == "y" || cmd == "y")
            {
                commands.Save(fileName);
            }
        }*/

        private static void ShowHelp()
        {
            Console.WriteLine("releaseIt [fileName]  for run.");
            Console.WriteLine("/s [fileName] to set config file.");
        }
    }
}