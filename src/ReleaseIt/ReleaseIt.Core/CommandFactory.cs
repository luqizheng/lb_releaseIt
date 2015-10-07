using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters;
using Newtonsoft.Json;

namespace ReleaseIt
{
    public class CommandFactory
    {
        private readonly IList<Command> _commands;

        public CommandFactory(IList<Command> command)
        {
            _commands = command;
        }
        public string SaveFile { get; set; }
        public CommandFactory()
        {
            _commands = new List<Command>();
        }

        public IList<Command> Commands
        {
            get { return _commands; }
        }

        public void Invoke(string folderExecuteFolder)
        {
            var executeFolder = Path.Combine(Environment.CurrentDirectory, folderExecuteFolder);
            if (!Directory.Exists(executeFolder))
            {
                (new DirectoryInfo(executeFolder)).CreateEx();
            }

            foreach (var cmd in _commands)
            {
                cmd.Invoke(executeFolder, this);
            }
        }

        public void Save(string file)
        {
            using (var writer = new StreamWriter(file))
            {
                var str = JsonConvert.SerializeObject(_commands, new JsonSerializerSettings
                {
                    TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple,
                    TypeNameHandling = TypeNameHandling.Auto,
                    Formatting = Formatting.Indented
                });
                writer.WriteLine(str);
            }
            this.SaveFile = file;
        }

        public CommandFactory Add(Command command)
        {
            _commands.Add(command);
            return this;
        }


        public static CommandFactory CreateFrom(string file)
        {
            if (file == null) throw new ArgumentNullException("file");
            if (!File.Exists(file))
                throw new FileNotFoundException("File not find", file);
            using (var reader = new StreamReader(file))
            {
                var json = reader.ReadToEnd();
                var commands = JsonConvert.DeserializeObject<IList<Command>>(json, new JsonSerializerSettings
                {
                    TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple,
                    TypeNameHandling = TypeNameHandling.Objects
                });
                return new CommandFactory(commands)
                {
                    SaveFile = file
                };

            }
        }

        public static CommandFactory Create()
        {
            return new CommandFactory();
        }
    }
}