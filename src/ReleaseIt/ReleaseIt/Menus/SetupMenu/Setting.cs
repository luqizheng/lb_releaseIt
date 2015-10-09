using ReleaseIt.MsBuilds;
using ReleaseIt.Publish;
using ReleaseIt.VersionControls;

namespace ReleaseIt.Menus.SetupMenu
{
    public abstract class Setting : Menu
    {

        private void SetupCommandBuildMenu()
        {
            _actions.Add(typeof(MsBuild), (cmd, cmdFactory, resultPath) =>
            {
                var setting = new MsBuildCommandMenu(WorkingFolder);
                setting.Modify(cmd, cmdFactory, resultPath);
            });

            _actions.Add(typeof(Svn), (cmd, cmdFactory, resultPath) =>
            {
                var setting = new VersionControlCommandMenu(WorkingFolder);
                setting.Modify(cmd, cmdFactory, resultPath);
            });
            _actions.Add(typeof(Git), (cmd, cmdFactory, resultPath) =>
            {
                var setting = new VersionControlCommandMenu(WorkingFolder);
                setting.Modify(cmd, cmdFactory, resultPath);
            });

            _actions.Add(typeof(XCopy), (cmd, cmdFactory, resultPath) =>
            {
                var setting = new XCopySetting(WorkingFolder);
                setting.Modify(cmd, cmdFactory, resultPath);
            });
        }

        public override Menu Do(CommandSet commandSet, string resultPath)
        {
            var cmd = CreateCommand();
            commandSet.Commands.Add(cmd);
            if(commandSet.Commands.Count>1)
            resultPath=
            Modify(commandSet.Commands.Count - 1, commandSet, resultPath);
            return Parent;
        }

        public abstract void Modify(int index, CommandSet commandSet, string resultPath);


        protected abstract Command CreateCommand();

        protected Setting(string workingFolder)
            : base(workingFolder)
        {
        }

    }
}