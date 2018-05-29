using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipelineFramework
{
    public sealed class AggregatePipelineTask<TContext> : IPipelineTask<TContext>
    {
        private readonly IEnumerable<IPipelineTask<TContext>> _tasks;
        private readonly List<IPipelineTask<TContext>> _otherTasks;

        public AggregatePipelineTask()
        {
            this._tasks = new List<IPipelineTask<TContext>>();
        }

        public AggregatePipelineTask(IEnumerable<IPipelineTask<TContext>> tasks)
        {
            this._tasks = tasks;
            this._otherTasks = new List<IPipelineTask<TContext>>();
        }

        public void EnqueueTask(IPipelineTask<TContext> task)
        {
            this._otherTasks.Add(task);
        }

        public async Task<IPipelineResult> ExecuteAsync(TContext context)
        {
            var results = new List<IPipelineResult>();

            foreach (var task in this._tasks.Concat(this._otherTasks))
            {
                results.Add(await task.ExecuteAsync(context));
            }

            return new AggregatePipelineResult(results);
        }
    }
}
