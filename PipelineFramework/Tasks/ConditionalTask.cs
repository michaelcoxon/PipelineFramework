using System;
using System.Threading.Tasks;

namespace PipelineFramework
{
    public sealed class ConditionalTask : IPipelineTask
    {
        public Func<bool> Rule { get; }
        public IPipelineTask Task { get; }

        public ConditionalTask(Func<bool> rule, IPipelineTask task)
        {
            this.Rule = rule;
            this.Task = task;
        }

        public async Task<IPipelineResult> ExecuteAsync()
        {
            if (this.Rule())
            {
                return new ConditionalTaskResult(await this.Task.ExecuteAsync());
            }
            else
            {
                return new ConditionalTaskResult(ConditionalTaskResultEnum.ConditionWasNotMatched);
            }
        }
    }

    public sealed  class ConditionalTask<TContext> : IPipelineTask<TContext>
    {
        public Func<TContext, bool> Rule { get; }
        public IPipelineTask<TContext> Task { get; }
        public TContext Context { get; }

        public ConditionalTask(TContext context, Func<TContext, bool> rule, IPipelineTask<TContext> task)
        {
            this.Context = context;
            this.Rule = rule;
            this.Task = task;
        }

        public async Task<IPipelineResult> ExecuteAsync()
        {
            if (this.Rule(this.Context))
            {
                return new ConditionalTaskResult(await this.Task.ExecuteAsync());
            }
            else
            {
                return new ConditionalTaskResult(ConditionalTaskResultEnum.ConditionWasNotMatched);
            }
        }
    }
}