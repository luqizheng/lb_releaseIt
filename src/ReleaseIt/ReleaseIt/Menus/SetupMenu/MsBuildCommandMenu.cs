using System;
using System.Collections.Generic;
using System.IO;
using ReleaseIt.MsBuilds;

namespace ReleaseIt.Menus.SetupMenu
{
    public class MsBuildCommandMenu : Setting
    {
        public MsBuildCommandMenu(string workingFolder)
            : base(workingFolder)
        {
        }

        public override string Key
        {
            get { return "m"; }
        }

        public override string Description
        {
            get { return "Set up Ms-build."; }
        }

        public override void Modify(int index1, CommandSet commandSet, string resultPath)
        {
            Console.WriteLine("For web project(y/n)?");
            var readLine = Console.ReadLine();

            var msbuild = (MsBuild)commandSet.Commands[index1];
            var y = readLine != null && readLine.ToLower().Trim() == "y";
            var builder = new MsBuildCommandBuilder(msbuild, y);
            var working = WorkingFolder ?? Environment.CurrentDirectory;
            var fileList = Find(working);

            for (var index = 0; index < fileList.Count; index++)
            {
                var file = fileList[index];
                Console.WriteLine(index + ":" + file);
            }

            Console.WriteLine("Please input the project file. e.g *.csproj or choice menuList");

            var cmd = Console.ReadLine();
            builder.ProjectPath(cmd);

            Console.WriteLine(
                "if you want to copy compiled folder to another place, please input the path, or press enter for noting.");
            var copyPath = Console.ReadLine();
            if (copyPath != null)
            {
                builder.CopyTo(copyPath);
            }
        }

        protected override Command CreateCommand()
        {
            return new MsBuild();
        }

        private IList<string> Find(string directory)
        {
            var result = new List<string>();
            var info = new DirectoryInfo(directory);
            var i = 1;
            foreach (var file in info.GetFiles("*.csproj", SearchOption.AllDirectories))
            {
                result.Add(file.FullName.Replace(directory, ""));
                i++;
            }
            return result;
        }
    }
}