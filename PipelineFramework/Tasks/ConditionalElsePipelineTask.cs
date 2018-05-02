using System;
using System.Threading.Tasks;

namespace PipelineFramework
{
    public sealed class ConditionalElsePipelineTask<TContext> : IPipelineTask<TContext>
    {
        public Func<TContext, bool> Condition { get; }

        public IPipelineTask<TContext> TrueTask { get; }

        public IPipelineTask<TContext> FalseTask { get; }

        public ConditionalElsePipelineTask(Func<TContext, bool> condition, IPipelineTask<TContext> trueTask, IPipelineTask<TContext> falseTask)
        {
            this.Condition = condition;
            this.TrueTask = trueTask;
            this.FalseTask = falseTask;
        }

        public async Task<IPipelineResult> ExecuteAsync(TContext context)
        {
            if (this.Condition(context))
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