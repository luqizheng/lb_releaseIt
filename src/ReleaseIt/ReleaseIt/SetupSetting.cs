namespace ReleaseIt
{
    public abstract class SetupSetting : Setting
    {
        public override Setting Execute(CommandFactory commandFactory)
        {
            var cmd = CreateCommand(commandFactory);
            commandFactory.Add(cmd);
            return this.Parent;
        }

        protected abstract Command CreateCommand(CommandFactory commandFactory);

    }
}