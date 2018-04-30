using System;
using System.Threading.Tasks;

namespace PipelineFramework
{
    public interface IPipelineTaskRuntime
    {
        DateTimeOffset EndTime { get; }
        IPipelineResult PipelineTaskResult { get; }
        DateTimeOffset StartTime { get; }
        IPipelineTask Task { get; }
        Task ExecuteAsync();
    }
}