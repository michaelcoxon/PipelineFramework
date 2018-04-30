using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PipelineFramework
{
    public sealed class PipelineTaskRuntime : IPipelineTaskRuntime
    {
        public IPipelineTask Task { get; }
        public IPipelineResult PipelineTaskResult { get; private set; }
        public DateTimeOffset StartTime { get; private set; }
        public DateTimeOffset EndTime { get; private set; }

        public PipelineTaskRuntime(IPipelineTask task)
        {
            this.Task = task;
        }

        public async Task ExecuteAsync()
        {
            this.StartTime = DateTimeOffset.Now;
            var result = await this.Task.ExecuteAsync();
            this.EndTime = DateTimeOffset.Now;

            this.PipelineTaskResult = result;
        }
    }
}
