using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ReleaseIt.Commands;

namespace ReleaseIt.IniStore
{
    internal static class SectionSettingMap
    {
        private static readonly Dictionary<string, Func<Setting>>
            Creator = new Dictionary<string, Func<Setting>>();
        private static readonly Dictionary<Type, string> SectionNames = new Dictionary<Type, string>();
        static SectionSettingMap()
        {
            Regist("build", () => new CompileSetting());


            Regist("git", () => new GitSetting());

            Regist("svn", () => new SvnSetting());

            Regist("copy", () => new CopySetting());
            Regist("smtp", () => new SmtpEmailSetting());
        }

        public static void Regist<T>(string name, Func<T> creatAction)
            where T : Setting
        {
            Creator.Add(name.ToLower(), creatAction);
            SectionNames.Add(typeof(T), name);
        }

        public static Setting Create(string name)
        {
            name = name.ToLower();
            return Creator[name]();
        }

        public static string GetSectionName(Type type)
        {
            return SectionNames[type];
        }

        public static IEnumerable<string> CommandTypes()
        {
            var result = new String[Creator.Count];
            var i = 0;
            foreach (var key in Creator.Keys)
            {
                result[i] = key;
                i++;
            }
            return result;
        }
    }
}