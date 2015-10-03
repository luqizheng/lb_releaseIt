using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using ReleaseIt.CommandFinders;

namespace ReleaseIt
{
    public class Envrionment
    {
        private readonly Dictionary<string, object> _values = new Dictionary<string, object>();


        public Envrionment()
        {
        }

        public T Get<T>(string name)
        {
            if (_values.ContainsKey(name))
            {
                return (T)_values[name];
            }
            throw new ArgumentOutOfRangeException("name");
        }

        public void Set(string name, object obj)
        {
            _values.Add(name, obj);
        }
    }
}