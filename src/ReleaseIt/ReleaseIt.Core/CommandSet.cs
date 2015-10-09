using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters;
using Newtonsoft.Json;

namespace ReleaseIt
{
    public class CommandSet
    {
        private readonly IList<ICommand> _commands;

        public CommandSet(IList<ICommand> command)
        {
            _commands = command;
        }

        public CommandSet()
        {
            _commands = new List<ICommand>();
        }

        public string File { get; private set; }

        public IList<ICommand> Commands
        {
            get { return _commands; }
        }

        public void Invoke(string workingFolder)
        {
            if (!Directory.Exists(workingFolder))
            {
                (new DirectoryInfo(workingFolder)).CreateEx();
            }

            var executeResult = new ExceuteResult(workingFolder);
            foreach (var cmd in _commands)
            {
                cmd.Invoke(executeResult, this);
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
            File = file;
        }


        public static CommandSet CreateFrom(string file)
        {
            if (file == null) throw new ArgumentNullException("file");
            if (!System.IO.File.Exists(file))
                throw new FileNotFoundException("File not find", file);
            using (var reader = new StreamReader(file))
            {
                var json = reader.ReadToEnd();
                var commands = JsonConvert.DeserializeObject<IList<ICommand>>(json, new JsonSerializerSettings
                {
                    TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple,
                    TypeNameHandling = TypeNameHandling.Objects
                });
                return new CommandSet(commands)
                {
                    File = file
                };
            }
        }

        public static CommandSet Create()
        {
            return new CommandSet();
        }
    }
}