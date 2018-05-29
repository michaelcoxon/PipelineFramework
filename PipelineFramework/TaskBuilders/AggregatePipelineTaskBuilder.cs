using EnsureFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipelineFramework
{
    public sealed class AggregatePipelineTaskBuilder<TParentPipelineTaskBuilder, TContext> :
        IAggregatePipelineTaskBuilder<TParentPipelineTaskBuilder, TContext, TContext>,
        IPipelineTaskBuilder<TContext>

        where TParentPipelineTaskBuilder : IPipelineTaskBuilder<TContext>
    {
        public bool Closed { get; private set; }

        public Queue<IPipelineTaskBuilder<TContext>> Tasks { get; }

        public TParentPipelineTaskBuilder Builder { get; }

        public TPipelineTaskBuilder EnqueueTask<TPipelineTaskBuilder>(TPipelineTaskBuilder taskBuilder)
            where TPipelineTaskBuilder : IPipelineTaskBuilder<TContext>
        {
            this.Tasks.Enqueue(taskBuilder);
            return taskBuilder;
        }

        public AggregatePipelineTaskBuilder(TParentPipelineTaskBuilder builder)
        {
            Ensure.Arg(builder, nameof(builder)).IsNotNull();

            this.Builder = builder;
            this.Tasks = new Queue<IPipelineTaskBuilder<TContext>>();
        }

        public void Close()
        {
            this.Closed = true;
        }

        public IPipelineTask<TContext> Build()
        {
            Ensure.Arg(this.Tasks, nameof(this.Tasks)).IsNotNullOrEmpty();

            return new AggregatePipelineTask<TContext>(this.Tasks.Select(b => b.Build()));
        }
    }

    public sealed class AggregatePipelineNewContextTaskBuilder<TParentPipelineTaskBuilder, TParentContext, TNewContext> :
        IAggregatePipelineTaskBuilder<TParentPipelineTaskBuilder, TParentContext, TNewContext>,
        IPipelineTaskBuilder<TNewContext>

        where TParentPipelineTaskBuilder : IPipelineTaskBuilder<TParentContext>
    {
        public bool Closed { get; private set; }

        public Queue<IPipelineTaskBuilder<TNewContext>> Tasks { get; }

        public TParentPipelineTaskBuilder Builder { get; }

        public TPipelineTaskBuilder EnqueueTask<TPipelineTaskBuilder>(TPipelineTaskBuilder taskBuilder)
            where TPipelineTaskBuilder : IPipelineTaskBuilder<TNewContext>
        {
            this.Tasks.Enqueue(taskBuilder);
            return taskBuilder;
        }

        public AggregatePipelineNewContextTaskBuilder(TParentPipelineTaskBuilder builder)
        {
            Ensure.Arg(builder, nameof(builder)).IsNotNull();

            this.Builder = builder;
            this.Tasks = new Queue<IPipelineTaskBuilder<TNewContext>>();
        }

        public void Close()
        {
            this.Closed = true;
        }

        public IPipelineTask<TNewContext> Build()
        {
            Ensure.Arg(this.Tasks, nameof(this.Tasks)).IsNotNullOrEmpty();

            return new AggregatePipelineTask<TNewContext>(this.Tasks.Select(b => b.Build()));
        }
    }
}
