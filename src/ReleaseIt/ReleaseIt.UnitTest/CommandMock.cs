using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReleaseIt.UnitTest
{
    public class CommandMock : ICommand
    {
        private readonly IList<string> _invokeResult;

        public CommandMock(IList<string> invokeResult)
        {
            _invokeResult = invokeResult;
        }

        public string Id { get; set; }
        public Setting Setting { get; set; }
        public bool SettingChanged { get; set; }
        public bool HasTag(IEnumerable<string> tag)
        {
            if (tag == null || this.Setting.Tags == null)
                return false;
            foreach (var a in tag)
            {
                foreach (var est in this.Setting.Tags)
                {
                    if (est == a)
                        return true;
                }
            }
            return false;
        }

        public ExecuteSetting Invoke(ExecuteSetting executeSetting)
        {
            var result = (ExecuteSetting)executeSetting.Clone();
            _invokeResult.Add(this.Id);
            result.SetVariable("OrderId", this.Id);
            return result;
        }

        public void OnOutput(string txt, ExecuteSetting setting)
        {
            throw new NotImplementedException();
        }

        public void OnErrorOutput(string txt, ExecuteSetting setting)
        {
            throw new NotImplementedException();
        }

        public string From { get; set; }

        public override string ToString()
        {
            return "Id=" + this.Id;
        }

        public object Clone()
        {
            return this;
        }
    }
}
