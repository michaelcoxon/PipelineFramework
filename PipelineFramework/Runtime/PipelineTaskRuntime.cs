using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PipelineFramework
{
    public sealed class PipelineTaskRuntime<TContext> : IPipelineTaskRuntime<TContext>
    {
        public IPipelineTask<TContext> Task { get; }
        public IPipelineResult PipelineTaskResult { get; private set; }
        public DateTimeOffset StartTime { get; private set; }
        public DateTimeOffset EndTime { get; private set; }

        public PipelineTaskRuntime(IPipelineTask<TContext> task)
        {
            this.Task = task;
        }

        public async Task ExecuteAsync(TContext context)
        {
            this.StartTime = DateTimeOffset.Now;
            var result = await this.Task.ExecuteAsync(context);
            this.EndTime = DateTimeOffset.Now;

            this.PipelineTaskResult = result;
        }
    }
}
