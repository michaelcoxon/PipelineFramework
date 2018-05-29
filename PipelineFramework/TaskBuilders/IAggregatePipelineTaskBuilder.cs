using System.Collections.Generic;

namespace PipelineFramework
{
    public interface IAggregatePipelineTaskBuilder<TParentPipelineTaskBuilder, TParentContext, TContext> : IPipelineTaskBuilder<TContext>
        where TParentPipelineTaskBuilder : IPipelineTaskBuilder<TParentContext>
    {
        TParentPipelineTaskBuilder Builder { get; }

        bool Closed { get; }

        Queue<IPipelineTaskBuilder<TContext>> Tasks { get; }

        void Close();

        TPipelineTaskBuilder EnqueueTask<TPipelineTaskBuilder>(TPipelineTaskBuilder taskBuilder)
            where TPipelineTaskBuilder : IPipelineTaskBuilder<TContext>;
    }
}