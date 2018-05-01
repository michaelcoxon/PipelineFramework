using EnsureFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace PipelineFramework
{
    public sealed class ConditionalTaskBuilder<TAggregatePipelineTaskBuilderParent, TContext> : IPipelineTaskBuilder<TContext>
        where TAggregatePipelineTaskBuilderParent : IPipelineTaskBuilder<TContext>
    {
        public Func<TContext, bool> Rule { get; set; }

        public AggregatePipelineTaskBuilder<ConditionalTaskBuilder<TAggregatePipelineTaskBuilderParent, TContext>, TContext> TrueTask { get; set; }

        public AggregatePipelineTaskBuilder<ConditionalTaskBuilder<TAggregatePipelineTaskBuilderParent, TContext>, TContext> FalseTask { get; set; }
        
        public AggregatePipelineTaskBuilder<TAggregatePipelineTaskBuilderParent, TContext> Builder { get; }

        public ConditionalTaskBuilder(AggregatePipelineTaskBuilder<TAggregatePipelineTaskBuilderParent, TContext> builder)
        {
            Ensure.Arg(builder, nameof(builder)).IsNotNull();

            this.Builder = builder;
        }

        public IPipelineTask<TContext> Build()
        {
            Ensure.Arg(this.Rule, nameof(this.Rule)).IsNotNull();
            Ensure.Arg(this.TrueTask, nameof(this.TrueTask)).IsNotNull();

            if (this.FalseTask == null)
            {
                return new ConditionalTask<TContext>(
                    this.Rule,
                    this.TrueTask.Build());
            }
            else
            {
                return new ConditionalElseTask<TContext>(
                    this.Rule,
                    this.TrueTask.Build(),
                    this.FalseTask.Build());
            }
        }

        IPipelineTaskBuilder<TContext> IPipelineTaskBuilder<TContext>.Builder => this.Builder;

    }
}
