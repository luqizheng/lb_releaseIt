using System.Collections.Generic;
using System.IO;
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

            var executeFolder = Path.Combine(System.Environment.CurrentDirectory, folderExecuteFolder);
            if (!Directory.Exists(executeFolder))
            {
                (new DirectoryInfo(executeFolder)).CreateEx();
            }

            foreach (var cmd in _commands)
            {
                cmd.Invoke(executeFolder);
            }
        }

        public void Save(string file)
        {
            using (var writer = new StreamWriter(file))
            {
                var str = JsonConvert.SerializeObject(_commands);
                writer.WriteLine(str);
            }
        }

        public CommandFactory Add(Command command)
        {
            _commands.Add(command);
            return this;
        }


        public static CommandFactory CreateFrom(string file)
        {
            var commands = JsonConvert.DeserializeObject<IList<Command>>(file);
            return new CommandFactory(commands);
        }

        public static CommandFactory Create()
        {
            return new CommandFactory();
        }
    }
}