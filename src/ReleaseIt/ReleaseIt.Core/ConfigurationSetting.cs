using System;
using System.Collections.Generic;
using System.Linq;
using ReleaseIt.Executors;
using ReleaseIt.Log;

namespace ReleaseIt
{
    public class ConfigurationSetting
    {


        public ConfigurationSetting()
        {
            ProcessLogger = new LoggerConsoler();
            CommandLogger = new LoggerConsoler();
        }

        public IExecutor Executor { get; set; }


        public ILog ProcessLogger { get; set; }

        public ILog CommandLogger { get; set; }

    }
}