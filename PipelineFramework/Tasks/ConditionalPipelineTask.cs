using System;
using System.Threading.Tasks;

namespace PipelineFramework
{
    public sealed  class ConditionalPipelineTask<TContext> : IPipelineTask<TContext>
    {
        public Func<TContext, bool> Condition { get; }
        public IPipelineTask<TContext> Task { get; }

        public ConditionalPipelineTask( Func<TContext, bool> condition, IPipelineTask<TContext> task)
        {
            this.Condition = condition;
            this.Task = task;
        }

        public async Task<IPipelineResult> ExecuteAsync(TContext context)
        {
            if (this.Condition(context))
            {
                return new ConditionalTaskResult(await this.Task.ExecuteAsync(context));
            }
            else
            {
                return new ConditionalTaskResult(ConditionalTaskResultEnum.ConditionWasNotMatched);
            }
        }
    }
}