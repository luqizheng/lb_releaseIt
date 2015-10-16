namespace ReleaseIt.WindowCommand.Publish
{
    public static class CopyToBuilder
    {
        public static CopySettingBuilder CopyTo(this CommandSet set, string path)
        {
            var r = new CopySetting();
            set.Add(r);
            r.TargetPath = path;
            return new CopySettingBuilder(r);
        }
    }
}