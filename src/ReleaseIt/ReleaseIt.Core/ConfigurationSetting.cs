using System;
using System.Collections.Generic;
using System.Linq;
using ReleaseIt.Executors;
using ReleaseIt.Log;

namespace ReleaseIt
{
    public class ConfigurationSetting
    {
        private readonly Dictionary<Type, Func<object, ICommand>> _builder =
            new Dictionary<Type, Func<object, ICommand>>();

        public ConfigurationSetting()
        {
            Logger = new LoggerConsoler();
        }

        public IExecutor Executor { get; set; }


        public ILog Logger { get; set; }

        public ICommand Create(object setting)
        {
            var type = setting.GetType();
            return _builder[type](setting);
        }

        public void Regist(Type settingType, Func<object, ICommand> func)
        {
            _builder.Add(settingType, func);
        }

        public Type[] GetRegistTypes()
        {
            return _builder.Keys.ToArray();
        }
    }
}