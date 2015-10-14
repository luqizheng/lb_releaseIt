using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Json;
using System.Text;
using Newtonsoft.Json;
using ReleaseIt.WindowCommand.Publish;

namespace ReleaseIt
{
    internal class Program
    {
        private static readonly JsonSerializerSettings setting = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            TypeNameHandling = TypeNameHandling.Auto,
            TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple
        };

        private static void Main(string[] args)
        {
            Console.Clear();
            Console.WriteLine("LB_Release 1.0");
            Console.WriteLine();
            Console.WriteLine();
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
            var commandSet = new CommandSet(fileInfo.Directory.FullName);
            From(fileInfo.FullName, commandSet);
            commandSet.Invoke();
        }

        private static void ShowSetting(string fileName)
        {
            //CreateTemplate

            var command = new CommandSet(new FileInfo(fileName).Directory.FullName);
            command.ForWidnow();

            command.Svn().Url("http://svn.address.com/trunk").Auth
                ("username", "password")
                .WorkingCopy("workongfolder");
            command.Build(true).Release().ProjectPath("/mypathfor.csproj").CopyTo("publish/%projName%");

            command.CopyTo("publish");

            Save(command, fileName);
        }

        private static void Save(CommandSet set, string filename)
        {
            // var commands = set.Commands;
            //var str = JsonConvert.SerializeObject(commands, setting);
            using (var stream = new MemoryStream())
            {
                var ser = new DataContractJsonSerializer(typeof (CommandSet), set.Setting.GetRegistTypes());
                ser.WriteObject(stream, set.Commands);

                using (var writer = new StreamWriter(filename))
                {
                    writer.Write(Encoding.UTF8.GetString(stream.ToArray()));
                }
            }
        }

        private static void From(string filename, CommandSet set)
        {
            using (var reader = File.OpenRead(filename))
            {
                var ser = new DataContractJsonSerializer(typeof (CommandSet), set.Setting.GetRegistTypes());
                var commands = (IList<ICommand>) ser.ReadObject(reader);
                foreach (var command in commands)
                    set.Commands.Add(command);
            }
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