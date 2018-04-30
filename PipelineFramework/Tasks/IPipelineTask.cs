using System;
using System.Threading.Tasks;

namespace PipelineFramework
{
    public interface IPipelineTask
    {
        Task<IPipelineResult> ExecuteAsync();
    }


    public interface IPipelineTask<TContext> : IPipelineTask
    {
        TContext Context { get; }
    }
}
