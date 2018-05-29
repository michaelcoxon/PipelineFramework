using EnsureFramework;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PipelineFramework
{
    public sealed class ActionTaskPipelineBuilder<TContext> : IPipelineTaskBuilder<TContext>
    {
        public Action<TContext> Action { get; set; }

        public IPipelineTask<TContext> Build()
        {
            Ensure.Arg(this.Action, nameof(this.Action)).IsNotNull();

            using (var logger = LoggerProvider.Create<ActionTaskPipelineBuilder<TContext>>())
            {
                logger.LogTrace($"Entering '{this.GetType()}.{nameof(this.Build)}'");

                var result = new ActionPipelineTask<TContext>(this.Action);

                logger.LogTrace($"Leaving '{this.GetType()}.{nameof(this.Build)}'");

                return result;
            }
        }
    }
}
