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

        public Setting Setting { get; set; }
        public bool SettingChanged { get; set; }
        public bool IsMatch(IEnumerable<string> tag)
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
            _invokeResult.Add(this.Setting.Id);
            result.AddVariable("OrderId", this.Setting.Id);
            return result;
        }

        public void OnOutput(string txt)
        {
            throw new NotImplementedException();
        }

        public void OnErrorOutput(string txt)
        {
            throw new NotImplementedException();
        }

        public string From { get; set; }

        public override string ToString()
        {
            return "Id=" + Setting.Id;
        }
    }
}
