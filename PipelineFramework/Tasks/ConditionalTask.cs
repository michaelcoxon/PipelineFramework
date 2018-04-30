using System;
using System.Threading.Tasks;

namespace PipelineFramework
{
    public sealed  class ConditionalTask<TContext> : IPipelineTask<TContext>
    {
        public Func<TContext, bool> Rule { get; }
        public IPipelineTask<TContext> Task { get; }

        public ConditionalTask( Func<TContext, bool> rule, IPipelineTask<TContext> task)
        {
            this.Rule = rule;
            this.Task = task;
        }

        public async Task<IPipelineResult> ExecuteAsync(TContext context)
        {
            if (this.Rule(context))
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