using System;
using System.Collections.Generic;
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
                if (!String.IsNullOrEmpty(setting.Name))
                {
                    seciontName = seciontName + "_" + setting.Name;
                }
                var s = file.Section(seciontName);
                SetToSection(s, setting);
            }

            file.Save(filename);
        }

        public void ReadSetting(CommandSet commandSet, string filename)
        {
            var file = new IniFile(filename);
            foreach (var section in file.Sections)
            {
                var sectionName = section.Name;

                var ary = sectionName.Split('_');
                sectionName = ary[0];
                string name = ary.Length > 1 ? ary[1] : null;

                var setting = Creator[sectionName]();
                setting.Name = name;
                FillProperities(setting, section);
                commandSet.Add(setting);
            }
        }

        private void FillProperities(Setting setting, IniSection section)
        {
            var iniProperties = section.Properties.ToDictionary(s => s.Name, s => s.Value);

            foreach (var propertyInfo in setting.GetType().GetProperties())
            {
                if (propertyInfo.Name == "Name")
                    continue;

                if (iniProperties.ContainsKey(propertyInfo.Name))
                {
                    var strValue = iniProperties[propertyInfo.Name];
                    if (propertyInfo.PropertyType == typeof(string))
                    {
                        propertyInfo.SetValue(setting, strValue);
                        continue;
                    }

                    if (propertyInfo.PropertyType.IsEnum)
                    {
                        propertyInfo.SetValue(setting, Enum.Parse(propertyInfo.PropertyType, strValue));
                        continue;
                    }

                    if (propertyInfo.PropertyType == typeof(int))
                    {
                        propertyInfo.SetValue(setting, Convert.ToInt32(strValue));
                        continue;
                    }

                    if (propertyInfo.PropertyType == typeof(bool))
                    {
                        propertyInfo.SetValue(setting, Convert.ToBoolean(strValue));
                        continue;
                    }
                    throw new ApplicationException(string.Format("Can't Convert {0} to type {1}", strValue,
                        propertyInfo.PropertyType.Name));
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