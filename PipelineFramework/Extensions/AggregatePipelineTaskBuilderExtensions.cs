using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipelineFramework
{
    public static class AggregatePipelineTaskBuilderExtensions
    {
        public static TPipelineTaskBuilder EndPipeline<TPipelineTaskBuilder, TContext>(this AggregatePipelineTaskBuilder<TPipelineTaskBuilder, TContext> builder)
            where TPipelineTaskBuilder : IPipelineTaskBuilder<TContext>
        {
            builder.Close();
            return builder.Builder;
        }
    }
}
