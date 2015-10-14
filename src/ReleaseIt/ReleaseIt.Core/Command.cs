using System;
using Newtonsoft.Json;
using ReleaseIt.WindowCommand.CommandFinders;

namespace ReleaseIt
{
    public abstract class Command : ICommand
    {
        protected Command(ICommandFinder finder)
        {
            if (finder == null) throw new ArgumentNullException("finder");
            Finder = finder;
        }

        [JsonIgnore]
        public ICommandFinder Finder { get; set; }


        //public ExecuteSetting Invoke(ExecuteSetting executeResult, CommandSet commandSet)
        //{
        //    var arguments = BuildParameters(executeResult)
        //        .Select(item => item.Build()).Where(arg => !string.IsNullOrEmpty(arg)).ToArray();
        //    Invoke(executeResult, arguments, commandSet);
        //    UpdateExecuteResultInfo(executeResult);
        //    return executeResult;
        //    ;
        //}


        ////protected abstract ICmdParameter[] BuildParameters(ExecuteSetting executeResult);

        //protected virtual void UpdateExecuteResultInfo(ExecuteSetting result)
        //{
        //}


        public abstract string BuildArguments(ExecuteSetting executoSetting);

        public string GetCommand(ExecuteSetting executorSetting)
        {
            return Finder.FindCmd();
        }
    }
}