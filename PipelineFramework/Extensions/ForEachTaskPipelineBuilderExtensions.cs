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
        public static ForEachPipelineTaskBuilder<TPipelineTaskBuilder, TContext, TElement> ForEach<TPipelineTaskBuilder, TContext, TElement>(
            this IAggregatePipelineTaskBuilder<TPipelineTaskBuilder, TContext, TContext> builder, Expression<Func<TContext, IEnumerable<TElement>>> selector)
          where TPipelineTaskBuilder : IPipelineTaskBuilder<TContext>
        {
            var task = new ForEachPipelineTaskBuilder<TPipelineTaskBuilder, TContext, TElement>(builder)
            {
                Selector = selector
            };

            builder.EnqueueTask(task);

            var aggBuilder = new AggregatePipelineNewContextTaskBuilder<ForEachPipelineTaskBuilder<TPipelineTaskBuilder, TContext, TElement>, TContext, IForEachTaskContext<TContext, TElement>>(task);
            task.Task = aggBuilder;

            return task;
        }

        public static IAggregatePipelineTaskBuilder<ForEachPipelineTaskBuilder<TPipelineTaskBuilder, TContext, TElement>, TContext, IForEachTaskContext<TContext, TElement>> BeginPipeline<TPipelineTaskBuilder, TContext, TElement>(this ForEachPipelineTaskBuilder<TPipelineTaskBuilder, TContext, TElement> builder)
           where TPipelineTaskBuilder : IPipelineTaskBuilder<TContext>
        {
            if (!builder.Task.Closed)
            {
                return builder.Task;
            }

            throw new Exception("Cannot begin a pipeline that is already closed");
        }

        public static IAggregatePipelineTaskBuilder<TPipelineTaskBuilder, TParentContext, TParentContext> EndForEach<TPipelineTaskBuilder, TParentContext, TElement>(
            this ForEachPipelineTaskBuilder<TPipelineTaskBuilder, TParentContext, TElement> builder)
            where TPipelineTaskBuilder : IPipelineTaskBuilder<TParentContext>
        {
            return builder.Builder;
        }
    }
}
