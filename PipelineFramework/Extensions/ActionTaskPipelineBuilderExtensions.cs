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
        public static AggregatePipelineTaskBuilder<TPipelineTaskBuilder, TContext> RunAction<TPipelineTaskBuilder, TContext>(this AggregatePipelineTaskBuilder<TPipelineTaskBuilder, TContext> builder, Action<TContext> action)
            where TPipelineTaskBuilder : IPipelineTaskBuilder<TContext>
        {
            var task = new ActionTaskPipelineBuilder<TContext>(builder)
            {
                Action = action
            };

            builder.EnqueueTask(task);

            return builder;
        }
    }
}
