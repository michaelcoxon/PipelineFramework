using EnsureFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PipelineFramework
{
    public sealed class ForEachPipelineTaskBuilder<TAggregatePipelineTaskBuilderParent, TContext, TElement> : IPipelineTaskBuilder<TContext>
        where TAggregatePipelineTaskBuilderParent : IPipelineTaskBuilder<TContext>
    {
        public bool Closed { get; private set; }

        public Expression<Func<TContext, IEnumerable<TElement>>> Selector { get; set; }

        public IAggregatePipelineTaskBuilder<ForEachPipelineTaskBuilder<TAggregatePipelineTaskBuilderParent, TContext, TElement>, TContext, IForEachTaskContext<TContext, TElement>> Task { get; set; }

        public IAggregatePipelineTaskBuilder<TAggregatePipelineTaskBuilderParent, TContext, TContext> Builder { get; }

        public ForEachPipelineTaskBuilder(IAggregatePipelineTaskBuilder<TAggregatePipelineTaskBuilderParent, TContext, TContext> builder)
        {
            this.Builder = builder;
        }

        public void Close()
        {
            this.Closed = true;
        }

        public IPipelineTask<TContext> Build()
        {
            Ensure.Arg(this.Task, nameof(this.Task)).IsNotNull();
            Ensure.Arg(this.Selector, nameof(this.Selector)).IsNotNull();

            return new ForEachTask<TContext, TElement>(this.Selector.Compile(), this.Task.Build());
        }
    }
}
