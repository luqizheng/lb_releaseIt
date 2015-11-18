using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReleaseIt.MultiExecute
{
    internal class CommandExecuteTree
    {


        private readonly CommandCollection _executeCommandList = new CommandCollection();

        private readonly CommandSet _commandSet;

        public CommandExecuteTree(CommandSet commandSet)
        {
            _commandSet = commandSet;
        }

        private void Fillter()
        {
            var _calledCommands = new List<string>();
            foreach (var command in _commandSet.Commands)
            {
                if (_calledCommands.Contains(command.Id)) // 排除被Call的Command
                    continue;
                _executeCommandList.Add(command);

                if (command.Setting.Call != null)
                {
                    _calledCommands.AddRange(command.Setting.Call);
                    foreach (var callCmdId in command.Setting.Call)
                    {

                        var callCmd = _commandSet.Commands[callCmdId];
                        var newCommand = (ICommand)callCmd.Clone();
                        newCommand.Id = command.Id + "_" + newCommand.Id;
                        newCommand.Setting.Dependency = command.Id;
                        _executeCommandList.Add(newCommand);
                    }
                }

            }
        }

        public Task BuildExecutePlan(out IEnumerable<Task> tasks)
        {
            Fillter();
            var sureTasks = new Dictionary<string, Task>();
            var lostParentTasks = new Dictionary<string, List<Action>>(); //Key is parent.
            TaskRecord startTask = null;
            foreach (var cmd in _executeCommandList)
            {
                if (!IsMatch(cmd)) continue;
                var action = CreateAction(cmd);

                if (cmd.Setting.Dependency == null && startTask == null)
                {
                    if (cmd.Id == null)
                    {
                        cmd.Id = "start";
                    }
                    startTask = new TaskRecord
                    {
                        Id = cmd.Id,
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
                    sureTasks.Add(cmd.Id, currentTask); //make  index
                }
                else
                {
                    //没有发现父节点,因此放入待处理区
                    if (!lostParentTasks.ContainsKey(depencyId))
                    {
                        lostParentTasks.Add(depencyId, new List<Action>());
                    }
                    lostParentTasks[depencyId].Add(action);
                }

                CheckLostParentTasks(lostParentTasks, sureTasks, cmd);
            }
            tasks = sureTasks.Values;
            return startTask.Task;
        }


        private void CheckLostParentTasks(Dictionary<string, List<Action>> lostParentTasks,
            Dictionary<string, Task> sureTasks, ICommand cmd)
        {
            if (lostParentTasks.ContainsKey(cmd.Id)) //新生成的task，看看是不是有人依赖他
            {
                var parentTask = sureTasks[cmd.Id];
                foreach (var childTask in lostParentTasks[cmd.Id])
                {
                    var task = childTask;
                    parentTask.ContinueWith(t => task);
                }
            }
        }

        /// <summary>
        ///     comd是否包含在这次的运行计划内
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        private bool IsMatch(ICommand cmd)
        {
            if ((_commandSet.Skip.Count != 0 && _commandSet.Skip.Contains(cmd.Id))
                ||
                (_commandSet.SkipTags.Count != 0 && cmd.HasTag(_commandSet.SkipTags))
                )
            {
                return false;
            }

            return (_commandSet.Include.Count == 0 && _commandSet.IncludeTags.Count == 0)
                   || _commandSet.Include.Contains(cmd.Id)
                   || cmd.HasTag(_commandSet.IncludeTags);
        }

        private Action CreateAction(ICommand executeCommand)
        {
            return () =>
            {
                try
                {
                    string resultCmdId = executeCommand.Setting.Dependency ?? CommandSet.DefaultExecuteSetting;
                    var executeSetting = _commandSet.ExecuteSettings[resultCmdId];
                    var resultSetting = executeCommand.Invoke(executeSetting);
                    _commandSet.ExecuteSettings.Add(executeCommand.Id, resultSetting);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            };
        }

        private class TaskRecord
        {
            public Task Task { get; set; }
            public string Id { get; set; }
        }
    }
}