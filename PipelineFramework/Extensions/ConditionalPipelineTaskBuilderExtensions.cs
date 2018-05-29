using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PipelineFramework
{
    public static class ConditionalPipelineTaskBuilderExtensions
    {
        public static ConditionalPipelineTaskBuilder<TPipelineTaskBuilder, TParentContext, TContext> If<TPipelineTaskBuilder, TParentContext, TContext>(this IAggregatePipelineTaskBuilder<TPipelineTaskBuilder, TParentContext, TContext> builder, Expression<Func<TContext, bool>> condition)
            where TPipelineTaskBuilder : IPipelineTaskBuilder<TParentContext>
        {
            var task = new ConditionalPipelineTaskBuilder<TPipelineTaskBuilder, TParentContext, TContext>(builder)
            {
                Condition = condition
            };

            builder.EnqueueTask(task);

            var aggBuilder = new AggregatePipelineTaskBuilder<ConditionalPipelineTaskBuilder<TPipelineTaskBuilder, TParentContext, TContext>, TContext>(task);
            task.TrueTask = aggBuilder;

            return task;
        }

        public static IAggregatePipelineTaskBuilder<ConditionalPipelineTaskBuilder<TPipelineTaskBuilder, TParentContext, TContext>, TContext, TContext> BeginPipeline<TPipelineTaskBuilder, TParentContext, TContext>(this ConditionalPipelineTaskBuilder<TPipelineTaskBuilder, TParentContext, TContext> builder)
            where TPipelineTaskBuilder : IPipelineTaskBuilder<TParentContext>
        {
            if (!builder.TrueTask.Closed)
            {
                return builder.TrueTask;
            }
            else if (builder.FalseTask != null && !builder.FalseTask.Closed)
            {
                return builder.FalseTask;
            }

            throw new Exception("Cannot begin a pipeline that is already closed");
        }

        public static IAggregatePipelineTaskBuilder<TPipelineTaskBuilder, TParentContext, TContext> EndIf<TPipelineTaskBuilder, TParentContext, TContext>(this ConditionalPipelineTaskBuilder<TPipelineTaskBuilder, TParentContext, TContext> builder)
            where TPipelineTaskBuilder : IPipelineTaskBuilder<TParentContext>
        {
            return builder.Builder;
        }

        public static ConditionalPipelineTaskBuilder<TPipelineTaskBuilder, TParentContext, TContext> Else<TPipelineTaskBuilder, TParentContext, TContext>(this ConditionalPipelineTaskBuilder<TPipelineTaskBuilder, TParentContext, TContext> builder)
            where TPipelineTaskBuilder : IPipelineTaskBuilder<TParentContext>
        {
            var aggBuilder = new AggregatePipelineTaskBuilder<ConditionalPipelineTaskBuilder<TPipelineTaskBuilder, TParentContext, TContext>, TContext>(builder);

            builder.FalseTask = aggBuilder;

            return builder;
        }
    }
}
