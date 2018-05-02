using EnsureFramework;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace PipelineFramework
{
    public sealed class ConditionalPipelineTaskBuilder<TAggregatePipelineTaskBuilderParent, TContext> : IPipelineTaskBuilder<TContext>
        where TAggregatePipelineTaskBuilderParent : IPipelineTaskBuilder<TContext>
    {
        public Expression<Func<TContext, bool>> Condition { get; set; }

        public AggregatePipelineTaskBuilder<ConditionalPipelineTaskBuilder<TAggregatePipelineTaskBuilderParent, TContext>, TContext> TrueTask { get; set; }

        public AggregatePipelineTaskBuilder<ConditionalPipelineTaskBuilder<TAggregatePipelineTaskBuilderParent, TContext>, TContext> FalseTask { get; set; }
        
        public AggregatePipelineTaskBuilder<TAggregatePipelineTaskBuilderParent, TContext> Builder { get; }

        public ConditionalPipelineTaskBuilder(AggregatePipelineTaskBuilder<TAggregatePipelineTaskBuilderParent, TContext> builder)
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

        IPipelineTaskBuilder<TContext> IPipelineTaskBuilder<TContext>.Builder => this.Builder;

    }
}
