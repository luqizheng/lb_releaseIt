using System;

namespace ReleaseIt
{
    public abstract class Menus : Setting
    {
        public override Setting Execute(CommandFactory commandFactory)
        {
            Console.Clear();
            foreach (var a in Commands.Values)
            {
                Console.WriteLine("[{0}] {1}", a.Key, a.Description);
            }
            Console.WriteLine("Press exit for exit and press back to back to parent menus.");
            while (true)
            {
                var cmd = Console.ReadLine().ToLower();
                if (cmd == "exit")
                {
                    return Setting.ExitSaveOrNot;
                }
                if (cmd == "parent")
                {
                    return Parent;
                }
                if (Commands.ContainsKey(cmd))
                {
                    return Commands[cmd];
                }
                Console.WriteLine("Not Recongiaze Cmd.");
            }
        }
    }
}