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
        public static IAggregatePipelineTaskBuilder<ConditionalPipelineTaskBuilder<TPipelineTaskBuilder, TParentContext, TContext>, TContext, TContext> If<TPipelineTaskBuilder, TParentContext, TContext>(
            this IAggregatePipelineTaskBuilder<TPipelineTaskBuilder, TParentContext, TContext> builder, 
            Expression<Func<TContext, bool>> condition)
            where TPipelineTaskBuilder : IPipelineTaskBuilder<TParentContext>
        {
            var task = new ConditionalPipelineTaskBuilder<TPipelineTaskBuilder, TParentContext, TContext>(builder)
            {
                Condition = condition
            };

            builder.EnqueueTask(task);

            var aggBuilder = new AggregatePipelineTaskBuilder<ConditionalPipelineTaskBuilder<TPipelineTaskBuilder, TParentContext, TContext>, TContext>(task);
            task.TrueTask = aggBuilder;

            return task.TrueTask;
        }

        public static IAggregatePipelineTaskBuilder<TPipelineTaskBuilder, TParentContext, TContext> EndIf<TPipelineTaskBuilder, TParentContext, TContext>(
            this IAggregatePipelineTaskBuilder<ConditionalPipelineTaskBuilder<TPipelineTaskBuilder, TParentContext, TContext>, TContext, TContext> builder)
            where TPipelineTaskBuilder : IPipelineTaskBuilder<TParentContext>
        {
            return builder.Builder.Builder;
        }

        public static IAggregatePipelineTaskBuilder<ConditionalPipelineTaskBuilder<TPipelineTaskBuilder, TParentContext, TContext>, TContext, TContext> Else<TPipelineTaskBuilder, TParentContext, TContext>(
            this IAggregatePipelineTaskBuilder<ConditionalPipelineTaskBuilder<TPipelineTaskBuilder, TParentContext, TContext>, TContext, TContext> builder)
            where TPipelineTaskBuilder : IPipelineTaskBuilder<TParentContext>
        {
            var aggBuilder = new AggregatePipelineTaskBuilder<ConditionalPipelineTaskBuilder<TPipelineTaskBuilder, TParentContext, TContext>, TContext>(builder.Builder);

            builder.Builder.FalseTask = aggBuilder;

            return builder.Builder.FalseTask;
        }
    }
}
