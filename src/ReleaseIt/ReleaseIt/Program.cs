using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml.Serialization;
using Newtonsoft.Json;
using ReleaseIt.IniStore;
using ReleaseIt.WindowCommand.Publish;

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
            var commandSet = From(fileInfo.FullName);
            commandSet.ForWidnow();
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
            SaveXml(command, fileName);

            //Save(command, fileName);
        }

        private static void SaveXml(CommandSet set, string fileName)
        {
            SettingManager manager = new SettingManager();
            manager.Save(set, fileName);
        }
        //private static void Save(CommandSet set, string filename)
        //{
        //    // var commands = set.Commands;
        //    //var str = JsonConvert.SerializeObject(commands, setting);
        //    using (var stream = new MemoryStream())
        //    {
        //        var ser = new DataContractJsonSerializer(typeof(IEnumerable<object>),
        //            set.Setting.GetRegistTypes());
        //        ser.WriteObject(stream, set.Commands.Select(s => s.Setting));

        //        using (var writer = new StreamWriter(filename))
        //        {
        //            writer.Write(Encoding.UTF8.GetString(stream.ToArray()).Replace(",", ",\r\n"));
        //        }
        //    }
        //}

        private static CommandSet From(string filename)
        {
            SettingManager manager = new SettingManager();
            return manager.From(filename);
            //using (var reader = File.OpenRead(filename))
            //{
            //    var ser = new DataContractJsonSerializer(typeof(IEnumerable<object>), set.Setting.GetRegistTypes());
            //    var commands = (IList<object>)ser.ReadObject(reader);
            //    foreach (var command in commands)
            //        set.Add(command);
            //}
        }


        private static void ShowHelp()
        {
            Console.WriteLine("releaseIt [fileName]  for run.");
            Console.WriteLine("/s [fileName] to set config file.");
        }
    }
}