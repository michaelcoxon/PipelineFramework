using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PipelineFramework
{
    public static class ActionTaskPipelineBuilderExtensions
    {
        public static IAggregatePipelineTaskBuilder<TPipelineTaskBuilder,TParentContext, TContext> RunAction<TPipelineTaskBuilder, TParentContext, TContext>(this IAggregatePipelineTaskBuilder<TPipelineTaskBuilder, TParentContext, TContext> builder, Action<TContext> action)
            where TPipelineTaskBuilder : IPipelineTaskBuilder<TParentContext>
        {
            var task = new ActionTaskPipelineBuilder<TContext>()
            {
                Action = action
            };

            builder.EnqueueTask(task);

            return builder;
        }
    }
}
