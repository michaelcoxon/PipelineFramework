using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace PipelineFramework
{
    public sealed class PipelineTaskBuilder<TContext> : IPipelineTaskBuilder<TContext>
    {
        private AggregatePipelineTaskBuilder<PipelineTaskBuilder<TContext>, TContext> _rootTask;

        public AggregatePipelineTaskBuilder<PipelineTaskBuilder<TContext>, TContext> BeginPipeline()
        {
            using (var logger = LoggerProvider.Create<PipelineTaskBuilder<TContext>>())
            {
                logger.LogTrace($"Entering '{this.GetType()}.{nameof(this.BeginPipeline)}'");

                if (this._rootTask != null)
                {
                    throw new Exception("Cannot begin pipeline more than once for a PipelineBuilder");
                }

                var result = this._rootTask = new AggregatePipelineTaskBuilder<PipelineTaskBuilder<TContext>, TContext>(this);
                logger.LogTrace($"Leaving '{this.GetType()}.{nameof(this.BeginPipeline)}'");
                return result;
            }
        }

        public IPipelineTask<TContext> Build()
        {
            using (var logger = LoggerProvider.Create<PipelineTaskBuilder<TContext>>())
            {
                logger.LogTrace($"Entering '{this.GetType()}.{nameof(this.Build)}'");
                var result = this._rootTask.Build();
                logger.LogTrace($"Leaving '{this.GetType()}.{nameof(this.Build)}'");
                return result;
            }

        }
    }
}
