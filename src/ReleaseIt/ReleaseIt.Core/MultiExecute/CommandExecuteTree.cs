using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReleaseIt.MultiExecute
{
    public class CommandExecuteTree
    {
        private readonly CommandSet _commands;

        public CommandExecuteTree(CommandSet commands)
        {
            _commands = commands;
        }

        public List<Task> BuildExecutePlan()
        {
            var result = new List<Task>();
            var makeSureList = new Dictionary<string, Task>();
            var unHandle = new Dictionary<string, List<Action>>(); //Key is parent.

            foreach (var cmd in _commands.Commands)
            {
                if (IsMatch(cmd))
                {
                    if (cmd.Setting.Dependency == null)
                    {
                        var commandTask = new Task(CreateAction(cmd));
                        makeSureList.Add(cmd.Setting.Id, commandTask);
                        result.Add(commandTask);
                    }

                    else if (makeSureList.ContainsKey(cmd.Setting.Dependency))
                    {
                        var action = CreateAction(cmd);
                        var parentTask = makeSureList[cmd.Setting.Dependency];
                        var currentTask = parentTask.ContinueWith(t => action());
                        makeSureList.Add(cmd.Setting.Id, currentTask); //make  index
                    }
                    else
                    {
                        var action = CreateAction(cmd);
                        //没有发现父节点,因此放入待处理区
                        if (!unHandle.ContainsKey(cmd.Setting.Dependency))
                        {
                            unHandle.Add(cmd.Setting.Dependency, new List<Action>());
                        }
                        unHandle[cmd.Setting.Dependency].Add(action);
                    }

                    //reset 待处理区 //看看代理处理去有缺父节点的command
                    if (unHandle.ContainsKey(cmd.Setting.Id)) //新生成的task，看看是不是有人依赖他
                    {
                        var parentTask = makeSureList[cmd.Setting.Id];
                        foreach (var childTask in unHandle[cmd.Setting.Id])
                        {
                            var task = childTask;
                            parentTask.ContinueWith(t => task);
                        }
                    }
                }
            }
            if (unHandle.Count != 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            return result;
        }

        private bool IsMatch(ICommand cmd)
        {
            if (_commands.Skip.Count != 0 && _commands.Skip.Contains(cmd.Setting.Id))
            {
                return false;
            }
            return (_commands.Include.Count == 0 && _commands.IncludeTags.Count == 0)
                   || _commands.Include.Contains(cmd.Setting.Id)
                   || cmd.IsMatch(_commands.IncludeTags);
        }

        private Action CreateAction(ICommand executeCommand)
        {
            return () =>
            {
                var resultSetting =
                    executeCommand.Invoke(
                        _commands.ExecuteSettings[executeCommand.Setting.Dependency ?? CommandSet.DefaultExecuteSetting]);
                _commands.ExecuteSettings.Add(executeCommand.Setting.Id, resultSetting);
            };
        }

        private void LoopPrepend(ICommand cmd, List<ICommand> result)
        {
            var backCount = 1;
            while (cmd != null && cmd.Setting.Dependency != null &&
                   cmd.Setting.Dependency != CommandSet.DefaultExecuteSetting)
            {
                if (result.All(s => s.Setting.Id != cmd.Setting.Dependency))
                {
                    var preCmd = _commands.Commands.FirstOrDefault(s => s.Setting.Id == cmd.Setting.Dependency);
                    if (preCmd != null)
                    {
                        result.Insert(result.Count - backCount, preCmd);
                        backCount++;
                        cmd = preCmd;
                        continue;
                    }
                }
                break;
            }
        }
    }
}