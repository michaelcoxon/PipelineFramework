using System;
using System.Threading.Tasks;

namespace PipelineFramework
{
    public interface IPipelineTaskRuntime<TContext>
    {
        DateTimeOffset EndTime { get; }
        IPipelineResult PipelineTaskResult { get; }
        DateTimeOffset StartTime { get; }
        IPipelineTask<TContext> Task { get; }
        Task ExecuteAsync(TContext context);
    }
}