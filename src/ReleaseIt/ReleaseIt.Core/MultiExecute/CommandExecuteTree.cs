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

        public Task BuildExecutePlan(out IEnumerable<Task> tasks)
        {
            Task result = null;
            var sureTasks = new Dictionary<string, Task>();
            var lostParentTasks = new Dictionary<string, List<Action>>(); //Key is parent.
            string startId = null;
            foreach (var cmd in _commands.Commands)
            {
                if (IsMatch(cmd))
                {
                    var action = CreateAction(cmd);
                    var depencyId = cmd.Setting.Dependency ?? startId;
                    if (depencyId == null)
                    {
                        if (result != null)
                        {
                            Console.WriteLine("Top continuteWith " + cmd.Setting.Id);

                            var parentTask = sureTasks[startId];
                            var currentTask = parentTask.ContinueWith(t => action());
                            Console.WriteLine(depencyId + " continuteWith " + cmd.Setting.Id);
                            sureTasks.Add(cmd.Setting.Id, currentTask); //make  index
                        }
                        else
                        {
                            result = new Task(action);
                            startId = cmd.Setting.Id;
                            sureTasks.Add(cmd.Setting.Id, result);
                        }
                    }

                    else if (sureTasks.ContainsKey(depencyId))
                    {
                        var parentTask = sureTasks[depencyId];
                        var currentTask = parentTask.ContinueWith(t => action());
                        Console.WriteLine(depencyId + " continuteWith " + cmd.Setting.Id);
                        sureTasks.Add(cmd.Setting.Id, currentTask); //make  index
                    }
                    else if (depencyId != null)
                    {
                        //没有发现父节点,因此放入待处理区
                        if (!lostParentTasks.ContainsKey(depencyId))
                        {
                            lostParentTasks.Add(depencyId, new List<Action>());
                        }
                        lostParentTasks[depencyId].Add(action);
                        lostParentTasks.Remove(depencyId);
                    }

                    //reset 待处理区 //看看代理处理去有缺父节点的command
                    if (lostParentTasks.ContainsKey(cmd.Setting.Id)) //新生成的task，看看是不是有人依赖他
                    {
                        var parentTask = sureTasks[cmd.Setting.Id];
                        foreach (var childTask in lostParentTasks[cmd.Setting.Id])
                        {
                            var task = childTask;
                            parentTask.ContinueWith(t => task);
                        }
                    }
                }
            }
            if (lostParentTasks.Count != 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            tasks = sureTasks.Values;
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
                var cmdId = executeCommand.Setting.Dependency ?? CommandSet.DefaultExecuteSetting;

                var executeSetting = _commands.ExecuteSettings[cmdId];
                var resultSetting = executeCommand.Invoke(executeSetting);
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