using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PipelineFramework
{
    public static class ConditionalTaskBuilderExtensions
    {
        public static ConditionalTaskBuilder<TPipelineTaskBuilder, TContext> If<TPipelineTaskBuilder, TContext>(this AggregatePipelineTaskBuilder<TPipelineTaskBuilder, TContext> builder, Func<TContext, bool> condition)
            where TPipelineTaskBuilder : IPipelineTaskBuilder<TContext>
        {
            var task = new ConditionalTaskBuilder<TPipelineTaskBuilder, TContext>(builder)
            {
                Rule = condition
            };

            builder.EnqueueTask(task);

            var aggBuilder = new AggregatePipelineTaskBuilder<ConditionalTaskBuilder<TPipelineTaskBuilder, TContext>, TContext>(task);
            task.TrueTask = aggBuilder;

            return task;
        }

        public static AggregatePipelineTaskBuilder<ConditionalTaskBuilder<TPipelineTaskBuilder, TContext>, TContext> BeginPipeline<TPipelineTaskBuilder, TContext>(this ConditionalTaskBuilder<TPipelineTaskBuilder, TContext> builder)
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

        public static AggregatePipelineTaskBuilder<TPipelineTaskBuilder, TContext> EndIf<TPipelineTaskBuilder, TContext>(this ConditionalTaskBuilder<TPipelineTaskBuilder, TContext> builder)
            where TPipelineTaskBuilder : IPipelineTaskBuilder<TContext>
        {
            return builder.Builder;
        }

        public static ConditionalTaskBuilder<TPipelineTaskBuilder, TContext> Else<TPipelineTaskBuilder, TContext>(this ConditionalTaskBuilder<TPipelineTaskBuilder, TContext> builder)
            where TPipelineTaskBuilder : IPipelineTaskBuilder<TContext>
        {
            var aggBuilder = new AggregatePipelineTaskBuilder<ConditionalTaskBuilder<TPipelineTaskBuilder, TContext>, TContext>(builder);

            builder.FalseTask = aggBuilder;

            return builder;
        }
    }
}
