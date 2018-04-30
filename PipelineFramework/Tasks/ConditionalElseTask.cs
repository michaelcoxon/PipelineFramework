using System;
using System.Threading.Tasks;

namespace PipelineFramework
{
    public sealed class ConditionalElseTask<TContext> : IPipelineTask<TContext>
    {
        public Func<TContext, bool> Rule { get; }

        public IPipelineTask<TContext> TrueTask { get; }

        public IPipelineTask<TContext> FalseTask { get; }

        public ConditionalElseTask(Func<TContext, bool> rule, IPipelineTask<TContext> trueTask, IPipelineTask<TContext> falseTask)
        {
            this.Rule = rule;
            this.TrueTask = trueTask;
            this.FalseTask = falseTask;
        }

        public async Task<IPipelineResult> ExecuteAsync(TContext context)
        {
            if (this.Rule(context))
            {
                return new ConditionalTaskResult(await this.TrueTask.ExecuteAsync(context));
            }
            else
            {
                return new ConditionalTaskResult(await this.FalseTask.ExecuteAsync(context));
            }
        }
    }
}