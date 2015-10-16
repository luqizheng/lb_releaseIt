namespace ReleaseIt
{
    public static class VcBuilderExtender
    {
        public static VCBuilder Svn(this CommandSet set)
        {
            var result = new VersionControlSetting
            {
                VersionControlType = VersionControlType.Svn
            };
            set.Add(result);
            return new VCBuilder(result);
        }

        public static VCBuilder Git(this CommandSet set)
        {
            var result = new VersionControlSetting
            {
                VersionControlType = VersionControlType.Git
            };
            set.Add(result);
            return new VCBuilder(result);
        }
    }
}