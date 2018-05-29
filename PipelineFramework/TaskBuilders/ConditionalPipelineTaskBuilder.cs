using EnsureFramework;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace PipelineFramework
{
    public sealed class ConditionalPipelineTaskBuilder<TAggregatePipelineTaskBuilderParent, TParentContext, TContext> : IPipelineTaskBuilder<TContext>
        where TAggregatePipelineTaskBuilderParent : IPipelineTaskBuilder<TParentContext>
    {
        public Expression<Func<TContext, bool>> Condition { get; set; }

        public IAggregatePipelineTaskBuilder<ConditionalPipelineTaskBuilder<TAggregatePipelineTaskBuilderParent, TParentContext, TContext>, TContext, TContext> TrueTask { get; set; }

        public IAggregatePipelineTaskBuilder<ConditionalPipelineTaskBuilder<TAggregatePipelineTaskBuilderParent, TParentContext, TContext>, TContext, TContext> FalseTask { get; set; }

        public IAggregatePipelineTaskBuilder<TAggregatePipelineTaskBuilderParent, TParentContext, TContext> Builder { get; }

        public ConditionalPipelineTaskBuilder(IAggregatePipelineTaskBuilder<TAggregatePipelineTaskBuilderParent, TParentContext, TContext> builder)
        {
            Ensure.Arg(builder, nameof(builder)).IsNotNull();
            this.Builder = builder;
        }

        public IPipelineTask<TContext> Build()
        {
            Ensure.Arg(this.Condition, nameof(this.Condition)).IsNotNull();
            Ensure.Arg(this.TrueTask, nameof(this.TrueTask)).IsNotNull();

            if (this.FalseTask == null)
            {
                return new ConditionalPipelineTask<TContext>(
                    this.Condition.Compile(),
                    this.TrueTask.Build());
            }
            else
            {
                return new ConditionalElsePipelineTask<TContext>(
                    this.Condition.Compile(),
                    this.TrueTask.Build(),
                    this.FalseTask.Build());
            }
        }
    }
}
