using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PipelineFramework
{
    public sealed class AggregatePipelineTask<TContext> : IPipelineTask<TContext>
    {
        private List<IPipelineTask<TContext>> _tasks;

        public AggregatePipelineTask()
        {
            this._tasks = new List<IPipelineTask<TContext>>();
        }

        public AggregatePipelineTask(IEnumerable<IPipelineTask<TContext>> tasks)
        {
            this._tasks = new List<IPipelineTask<TContext>>(tasks);
        }

        public void EnqueueTask(IPipelineTask<TContext> task)
        {
            this._tasks.Add(task);
        }

        public async Task<IPipelineResult> ExecuteAsync(TContext context)
        {
            var results = new List<IPipelineResult>();

            foreach (var task in this._tasks)
            {
                results.Add(await task.ExecuteAsync(context));
            }

            return new AggregatePipelineResult(results);
        }
    }
}
