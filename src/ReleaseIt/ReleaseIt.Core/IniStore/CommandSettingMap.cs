using System;
using System.Collections.Generic;
using System.Linq;

namespace ReleaseIt.IniStore
{
    public static class CommandSettingMap
    {
        private static readonly
            Dictionary<Type, Func<Setting, ICommand>> _builder =
                new Dictionary<Type, Func<Setting, ICommand>>();


        public static ICommand Create(Setting setting)
        {
            var type = setting.GetType();
            return _builder[type](setting);
        }

        public static void Regist(Type settingType, Func<object, ICommand> func)
        {
            _builder.Add(settingType, func);
        }

        public static Type[] GetRegistTypes()
        {
            return _builder.Keys.ToArray();
        }
    }
}