using System;
using ReleaseIt.Publish;

namespace ReleaseIt.Menus.SetupMenu
{
    public class XCopySetting : Setting
    {
        public override string Key
        {
            get { return "x"; }
        }

        public override string Description
        {
            get { return "xCopy"; }
        }

        public override void Modify(int index, CommandSet commandSet, string resultPath)
        {
            var result = (XCopy)commandSet.Commands[index];
            while (string.IsNullOrEmpty(result.TargetPath))
            {
                Console.WriteLine("Input target path.");
                result.TargetPath = Console.ReadLine();
            }

            while (true)
            {
                try
                {
                    Console.WriteLine("1. Full Copy, 2.Base on copy Date Y-M-D?");
                    var choice = Convert.ToInt32(Console.ReadLine());
                    result.UserDateCompareCopy = choice == 2;
                    break;
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }

        protected override Command CreateCommand()
        {
            var result = new XCopy();

            return result;
        }

        public XCopySetting(string workingFolder)
            : base(workingFolder)
        {
        }
    }
}