using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PipelineFramework
{
    public static class ForeachTaskPipelineBuilderExtensions
    {

        public static ForEachPipelineTaskBuilder<TPipelineTaskBuilder, TParentContext, TContext, TElement> ForEach<TPipelineTaskBuilder, TParentContext, TContext, TElement>(
            this IAggregatePipelineTaskBuilder<TPipelineTaskBuilder, TParentContext, TContext> builder,
            Expression<Func<TContext, IEnumerable<TElement>>> selector)
            where TPipelineTaskBuilder : IPipelineTaskBuilder<TParentContext>
        {
            var task = new ForEachPipelineTaskBuilder<TPipelineTaskBuilder, TParentContext, TContext, TElement>(builder)
            {
                Selector = selector
            };

            builder.EnqueueTask(task);

            var aggBuilder = new AggregatePipelineNewContextTaskBuilder<ForEachPipelineTaskBuilder<TPipelineTaskBuilder, TParentContext, TContext, TElement>, TContext, IForEachTaskContext<TContext, TElement>>(task);
            task.Task = aggBuilder;

            return task;
        }

        public static IAggregatePipelineTaskBuilder<ForEachPipelineTaskBuilder<TPipelineTaskBuilder, TParentContext, TContext, TElement>, TContext, IForEachTaskContext<TContext, TElement>> BeginPipeline<TPipelineTaskBuilder, TParentContext, TContext, TElement>(
            this ForEachPipelineTaskBuilder<TPipelineTaskBuilder, TParentContext, TContext, TElement> builder)
            where TPipelineTaskBuilder : IPipelineTaskBuilder<TParentContext>
        {
            if (!builder.Task.Closed)
            {
                return builder.Task;
            }

            throw new Exception("Cannot begin a pipeline that is already closed");
        }


        public static IAggregatePipelineTaskBuilder<TPipelineTaskBuilder, TParentContext, TContext> EndForEach<TPipelineTaskBuilder, TParentContext, TContext, TElement>(
            this ForEachPipelineTaskBuilder<TPipelineTaskBuilder, TParentContext, TContext, TElement> builder)
            where TPipelineTaskBuilder : IPipelineTaskBuilder<TParentContext>
        {
            return builder.Builder;
        }
    }
}
