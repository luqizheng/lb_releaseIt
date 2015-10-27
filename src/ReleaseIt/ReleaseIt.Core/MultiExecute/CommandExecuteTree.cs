using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReleaseIt.MultiExecute
{
    internal class CommandExecuteTree
    {
        private readonly CommandSet _commands;

        public CommandExecuteTree(CommandSet commands)
        {
            _commands = commands;
        }


        public Task BuildExecutePlan(out IEnumerable<Task> tasks)
        {
            var sureTasks = new Dictionary<string, Task>();
            var lostParentTasks = new Dictionary<string, List<Action>>(); //Key is parent.
            TaskRecord startTask = null;
            foreach (var cmd in _commands.Commands)
            {
                if (!IsMatch(cmd)) continue;
                var action = CreateAction(cmd);

                if (cmd.Setting.Dependency == null && startTask == null)
                {
                    if (cmd.Setting.Id == null)
                    {
                        cmd.Setting.Id = "start";
                    }
                    startTask = new TaskRecord
                    {
                        Id = cmd.Setting.Id,
                        Task = new Task(action)
                    };

                    sureTasks.Add(startTask.Id, startTask.Task);
                    CheckLostParentTasks(lostParentTasks, sureTasks, cmd);
                    continue;
                }
                var depencyId = cmd.Setting.Dependency ?? startTask.Id;
                if (sureTasks.ContainsKey(depencyId))
                {
                    var parentTask = sureTasks[depencyId];
                    var currentTask = parentTask.ContinueWith(t => action());
                    sureTasks.Add(cmd.Setting.Id, currentTask); //make  index
                }
                else
                {
                    //没有发现父节点,因此放入待处理区
                    if (!lostParentTasks.ContainsKey(depencyId))
                    {
                        lostParentTasks.Add(depencyId, new List<Action>());
                    }
                    lostParentTasks[depencyId].Add(action);
                    lostParentTasks.Remove(depencyId);
                }

                CheckLostParentTasks(lostParentTasks, sureTasks, cmd);
            }
            tasks = sureTasks.Values;
            return startTask.Task;
        }

        private void CheckLostParentTasks(Dictionary<string, List<Action>> lostParentTasks,
            Dictionary<string, Task> sureTasks, ICommand cmd)
        {
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

        private bool IsMatch(ICommand cmd)
        {
            if ((_commands.Skip.Count != 0 && _commands.Skip.Contains(cmd.Setting.Id))
                ||
                (_commands.SkipTags.Count != 0 && cmd.HasTag(_commands.SkipTags))
                )
            {
                return false;
            }

            return (_commands.Include.Count == 0 && _commands.IncludeTags.Count == 0)
                   || _commands.Include.Contains(cmd.Setting.Id)
                   || cmd.HasTag(_commands.IncludeTags);
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

        private class TaskRecord
        {
            public Task Task { get; set; }
            public string Id { get; set; }
        }
    }
}