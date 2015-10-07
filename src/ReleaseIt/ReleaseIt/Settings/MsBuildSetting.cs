using System;
using ReleaseIt.MsBuilds;

namespace ReleaseIt.Settings
{
    public class MsBuildSetting : SetupSetting
    {
        public override string Key
        {
            get { return "m"; }
        }

        public override string Description
        {
            get { return "Set up Ms-build."; }
        }

        protected override Command CreateCommand(CommandFactory commandFactory)
        {
            var msbuild = new MsBuild();
            Console.WriteLine("For web project(y/n)?");
            var y = Console.ReadLine().ToLower().Trim() == "y";
            var builder = new MsBuildBuilder(msbuild, y);

            Console.WriteLine("Please input the project file. e.g *.csproj ");
            var cmd = Console.ReadLine();
            builder.ProjectPath(cmd);

            Console.WriteLine(
                "if you want to copy compiled folder to another place,please input the path, or input N to do nothing.");
            var copyPath = Console.ReadLine().Trim();
            if (copyPath.Length != 1 && copyPath.Substring(0, 1).ToLower() != "n")
            {
                builder.CopyTo(copyPath);
            }

            return msbuild;
        }
    }
}