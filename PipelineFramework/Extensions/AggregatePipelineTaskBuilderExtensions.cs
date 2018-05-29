using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipelineFramework
{
    public static class AggregatePipelineTaskBuilderExtensions
    {
        public static TPipelineTaskBuilder EndPipeline<TPipelineTaskBuilder, TParentContext, TContext>(this IAggregatePipelineTaskBuilder<TPipelineTaskBuilder, TParentContext, TContext> builder)
            where TPipelineTaskBuilder : IPipelineTaskBuilder<TParentContext>
        {
            builder.Close();
            return builder.Builder;
        }
    }
}
