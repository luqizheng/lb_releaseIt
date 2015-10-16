using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ReleaseIt.Commands;

namespace ReleaseIt.IniStore
{
    public class SettingManager
    {
        private static readonly Dictionary<string, Func<Setting>>
            Creator = new Dictionary<string, Func<Setting>>();

        static SettingManager()
        {
            Creator.Add(typeof(BuildSetting).Name, () => new BuildSetting());
            Creator.Add(typeof(VersionControlSetting).Name, () => new VersionControlSetting());
            Creator.Add(typeof(CopySetting).Name, () => new CopySetting());
            Creator.Add(typeof(EmailSetting).Name, () => new EmailSetting());
        }

        public void Save(CommandSet commandSet, string filename)
        {
            var file = new IniFile();

            foreach (var setting in commandSet.Commands.Select(s => s.Setting))
            {
                var seciontName = setting.GetType().Name;
                var s = file.Section(seciontName);
                s.Set("Name", setting.Name);
                SetToSection(s, setting);
            }

            file.Save(filename);
        }

        public void ReadSetting(CommandSet commandSet, string filename)
        {

            var file = new IniFile(filename);
            foreach (var section in file.Sections)
            {
                var setting = Creator[section.Name]();
                FullProperty(setting, section);
                commandSet.Add(setting);
            }
        }

        private void FullProperty(Setting setting, IniSection section)
        {
            var keyMap = section.Properties.ToDictionary(s => s.Name, s => s.Value);
            foreach (var item in setting.GetType().GetProperties())
            {
                if (keyMap.ContainsKey(item.Name))
                {
                    var strValue = keyMap[item.Name];
                    if (item.PropertyType == typeof(string))
                    {
                        item.SetValue(setting, strValue);
                        continue;
                    }

                    if (item.PropertyType.IsEnum)
                    {
                        item.SetValue(setting, Enum.Parse(item.PropertyType, strValue));
                        continue;
                    }

                    if (item.PropertyType == typeof(int))
                    {
                        item.SetValue(setting, Convert.ToInt32(strValue));
                        continue;
                    }

                    if (item.PropertyType == typeof(bool))
                    {
                        item.SetValue(setting, Convert.ToBoolean(strValue));
                        continue;
                    }
                    throw new ApplicationException(string.Format("Can't Convert {0} to type {1}", strValue, item.PropertyType.Name));
                }
            }
        }

        private void SetToSection(IniSection section, Setting s)
        {
            foreach (var propInfo in s.GetType().GetProperties())
            {
                if (propInfo.Name == "Name")
                    continue;

                section.Set(propInfo.Name, Convert.ToString(propInfo.GetValue(s)));
            }
        }
    }
}