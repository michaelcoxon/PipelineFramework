using System;
using System.Threading.Tasks;

namespace PipelineFramework
{
    public interface IPipelineTask<TContext>
    {
        Task<IPipelineResult> ExecuteAsync(TContext context);
    }
}
