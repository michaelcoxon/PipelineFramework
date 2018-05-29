using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipelineFramework
{
    public sealed class PipelineTaskBuilder<TContext> : IPipelineTaskBuilder<TContext>
    {
        private AggregatePipelineTaskBuilder<PipelineTaskBuilder<TContext>, TContext> _rootTask;

        public AggregatePipelineTaskBuilder<PipelineTaskBuilder<TContext>, TContext> BeginPipeline()
        {
            if (this._rootTask != null)
            {
                throw new Exception("Cannot begin pipeline more than once for a PipelineBuilder");
            }

            return this._rootTask = new AggregatePipelineTaskBuilder<PipelineTaskBuilder<TContext>, TContext>(this);
        }

        public IPipelineTask<TContext> Build()
        {
            return this._rootTask.Build();
        }
    }
}
