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
            Creator.Add(typeof (BuildSetting).Name, () => new BuildSetting());
            Creator.Add(typeof (VersionControlSetting).Name, () => new VersionControlSetting());
            Creator.Add(typeof (CopySetting).Name, () => new CopySetting());
            Creator.Add(typeof (EmailSetting).Name, () => new EmailSetting());
        }

        public void Save(CommandSet commandSet, string filename)
        {
            var file = new IniFile();

            foreach (var setting in commandSet.Commands.Select(s => s.Setting))
            {
                var seciontName = setting.GetType().Name;
                if (!string.IsNullOrEmpty(setting.Id))
                {
                    seciontName = seciontName + "_" + setting.Id;
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
                var name = ary.Length > 1 ? string.Join("_", ary, 1, ary.Length - 2) : null;

                var setting = Creator[sectionName]();
                setting.Id = name;
                FillProperities(setting, section);
                commandSet.Add(setting);
            }
        }

        private void FillProperities(Setting setting, IniSection section)
        {
            var iniProperties = section.Properties.ToDictionary(s => s.Name, s => s.Value);

            foreach (var propertyInfo in setting.GetType().GetProperties())
            {
                if (propertyInfo.Name == "Id")
                    continue;

                if (iniProperties.ContainsKey(propertyInfo.Name))
                {
                    var strValue = iniProperties[propertyInfo.Name];
                    if (propertyInfo.PropertyType == typeof (string))
                    {
                        propertyInfo.SetValue(setting, strValue);
                        continue;
                    }

                    if (propertyInfo.PropertyType.IsEnum)
                    {
                        propertyInfo.SetValue(setting, Enum.Parse(propertyInfo.PropertyType, strValue));
                        continue;
                    }

                    if (propertyInfo.PropertyType == typeof (int))
                    {
                        propertyInfo.SetValue(setting, Convert.ToInt32(strValue));
                        continue;
                    }

                    if (propertyInfo.PropertyType == typeof (bool))
                    {
                        propertyInfo.SetValue(setting, Convert.ToBoolean(strValue));
                        continue;
                    }

                    if (propertyInfo.PropertyType.IsArray &&
                        propertyInfo.PropertyType.GetElementType() == typeof (string))
                    {
                        var aryStrValue = strValue.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);
                        propertyInfo.SetValue(setting, Convert.ToString(aryStrValue));
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
                if (propInfo.Name == "Id")
                    continue;
                if (propInfo.PropertyType.IsArray)
                {
                    var ary = (Array) propInfo.GetValue(s);
                    if (ary == null)
                        continue;
                    IList<string> aryJoin = (from object ele in ary select Convert.ToString(ele)).ToList();

                    section.Set(propInfo.Name, string.Join(",", aryJoin.ToArray()));
                    continue;
                }
                section.Set(propInfo.Name, Convert.ToString(propInfo.GetValue(s)));
            }
        }
    }
}