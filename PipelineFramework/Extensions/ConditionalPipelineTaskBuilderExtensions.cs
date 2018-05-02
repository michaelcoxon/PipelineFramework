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
        public static ConditionalPipelineTaskBuilder<TPipelineTaskBuilder, TContext> If<TPipelineTaskBuilder, TContext>(this AggregatePipelineTaskBuilder<TPipelineTaskBuilder, TContext> builder, Expression<Func<TContext, bool>> condition)
            where TPipelineTaskBuilder : IPipelineTaskBuilder<TContext>
        {
            var task = new ConditionalPipelineTaskBuilder<TPipelineTaskBuilder, TContext>(builder)
            {
                Condition = condition
            };

            builder.EnqueueTask(task);

            var aggBuilder = new AggregatePipelineTaskBuilder<ConditionalPipelineTaskBuilder<TPipelineTaskBuilder, TContext>, TContext>(task);
            task.TrueTask = aggBuilder;

            return task;
        }

        public static AggregatePipelineTaskBuilder<ConditionalPipelineTaskBuilder<TPipelineTaskBuilder, TContext>, TContext> BeginPipeline<TPipelineTaskBuilder, TContext>(this ConditionalPipelineTaskBuilder<TPipelineTaskBuilder, TContext> builder)
            where TPipelineTaskBuilder : IPipelineTaskBuilder<TContext>
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

        public static AggregatePipelineTaskBuilder<TPipelineTaskBuilder, TContext> EndIf<TPipelineTaskBuilder, TContext>(this ConditionalPipelineTaskBuilder<TPipelineTaskBuilder, TContext> builder)
            where TPipelineTaskBuilder : IPipelineTaskBuilder<TContext>
        {
            return builder.Builder;
        }

        public static ConditionalPipelineTaskBuilder<TPipelineTaskBuilder, TContext> Else<TPipelineTaskBuilder, TContext>(this ConditionalPipelineTaskBuilder<TPipelineTaskBuilder, TContext> builder)
            where TPipelineTaskBuilder : IPipelineTaskBuilder<TContext>
        {
            var aggBuilder = new AggregatePipelineTaskBuilder<ConditionalPipelineTaskBuilder<TPipelineTaskBuilder, TContext>, TContext>(builder);

            builder.FalseTask = aggBuilder;

            return builder;
        }
    }
}
