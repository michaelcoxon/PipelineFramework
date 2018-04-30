using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PipelineFramework
{
    public sealed class AggregatePipelineTask : IPipelineTask
    {
        private List<IPipelineTask> _tasks;

        public AggregatePipelineTask()
        {
            this._tasks = new List<IPipelineTask>();
        }

        public AggregatePipelineTask(IEnumerable<IPipelineTask> tasks)
        {
            this._tasks = new List<IPipelineTask>(tasks);
        }

        public void EnqueueTask(IPipelineTask task)
        {
            this._tasks.Add(task);
        }

        public async Task<IPipelineResult> ExecuteAsync()
        {
            var results = new List<IPipelineResult>();

            foreach (var task in this._tasks)
            {
                results.Add(await task.ExecuteAsync());
            }

            return new AggregatePipelineResult(results);
        }
    }
}
