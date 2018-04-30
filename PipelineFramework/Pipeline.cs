using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PipelineFramework
{


    public sealed class Pipeline
    {
        private TaskLinkedListItem _rootTask;
        private TaskLinkedListItem _lastTask;

        private sealed class TaskLinkedListItem
        {
            public IPipelineTask Task { get; }
            public TaskLinkedListItem NextTask { get; set; }

            public TaskLinkedListItem(IPipelineTask task)
            {
                this.Task = task;
            }
        }

        public void EnqueueTask(IPipelineTask task)
        {
            if (this._rootTask == null)
            {
                this._rootTask = this._lastTask = new TaskLinkedListItem(task);
            }
            else
            {
                var newTask = new TaskLinkedListItem(task);
                this._lastTask.NextTask = newTask;
                this._lastTask = newTask;
            }
        }

        public async Task<AggregatePipelineResult> ExecuteAsync()
        {
            var results = new List<IPipelineResult>();

            var task = this._rootTask;

            do
            {
                results.Add(await task.Task.ExecuteAsync());
                task = task.NextTask;
            }
            while (task != null);

            return new AggregatePipelineResult(results);
        }
    }
}
