using System;
using System.Collections.Generic;
using System.IO;

namespace ReleaseIt
{
    internal class Program
    {

        private static Setting Current;
        private static void Main(string[] args)
        {
            Console.Clear();
            Console.WriteLine("LB_Release 1.0");
            Console.WriteLine();
            Console.WriteLine();
            var fileName = "";
            var parameter = "";
            foreach (var arg in args)
            {
                if (arg.StartsWith("/"))
                {
                    if (arg != "/s")
                    {
                        ShowHelp();
                        return;
                    }
                }

                if (parameter == "")
                {
                    parameter = arg;
                }
                if (fileName == "")
                {
                    fileName = arg;
                }
            }
            if (parameter == "")
            {
                CommandFactory.CreateFrom(fileName).Invoke(Environment.CurrentDirectory);
            }
            else
            {
                ShowSetting(fileName);
            }
        }


        private static void ShowSetting(string fileName)
        {

            CommandFactory commands =
                File.Exists(fileName)
                    ? CommandFactory.CreateFrom(fileName)
                    : new CommandFactory();

            Current = new Top();
            while (true)
            {
                Current = Current.Execute(commands);
                if (Current == null)
                    break;
            }
            Console.WriteLine();
            Console.WriteLine(Setting.ExitSaveOrNot.Description);
            var lastCmd = Console.ReadLine().ToLower().Trim();
            if (lastCmd == "s")
            {
                commands.Save(fileName);
            }
        }

        private static void ShowHelp()
        {
            Console.WriteLine("releaseIt [fileName]  for run.");
            Console.WriteLine("/s [fileName] to set config file.");
        }
    }
}