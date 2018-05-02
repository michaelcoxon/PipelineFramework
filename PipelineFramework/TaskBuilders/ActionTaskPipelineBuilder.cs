using EnsureFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipelineFramework
{
    public sealed class ActionTaskPipelineBuilder<TContext> : IPipelineTaskBuilder<TContext>
    {
        public Action<TContext> Action { get; set; }

        public IPipelineTaskBuilder<TContext> Builder { get; }

        public ActionTaskPipelineBuilder(IPipelineTaskBuilder<TContext> builder)
        {
            Ensure.Arg(builder, nameof(builder)).IsNotNull();

            this.Builder = builder;
        }

        public IPipelineTask<TContext> Build()
        {
            Ensure.Arg(this.Action, nameof(this.Action)).IsNotNull();

            return new ActionPipelineTask<TContext>(this.Action);
        }
    }
}
