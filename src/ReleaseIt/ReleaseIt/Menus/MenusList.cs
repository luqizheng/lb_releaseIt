using System;

namespace ReleaseIt.Menus
{
    public abstract class MenusList : Menu
    {
        public override Menu Do(CommandSet commandSet, string resultPath)
        {
            Console.Clear();
            foreach (var a in Settings.Values)
            {
                Console.WriteLine("[{0}] {1}", a.Key, a.Description);
            }
            Console.WriteLine("Press exit for exit and press back to back to parent menus.");
            while (true)
            {
                var cmd = Console.ReadLine().ToLower();
                if (cmd == "exit")
                {
                    return Menu.ExitSaveOrNot;
                }
                if (cmd == "parent")
                {
                    return Parent;
                }
                if (Settings.ContainsKey(cmd))
                {
                    return Settings[cmd];
                }
                Console.WriteLine("Not Recongiaze Cmd.");
            }
        }


        public MenusList(string workingFolder)
            : base(workingFolder)
        {
        }
    }
}