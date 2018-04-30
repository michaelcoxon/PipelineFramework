using EnsureFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace PipelineFramework.Builders
{
    public class ConditionalTaskBuilder<TContext> : IPipelineTaskBuilder<TContext>
    {
        public Func<TContext, bool> Rule { get; set; }

        public IPipelineTaskBuilder<TContext> TrueTask { get; set; }

        public IPipelineTaskBuilder<TContext> FalseTask { get; set; }

        public IPipelineTask<TContext> Build()
        {
            Ensure.Arg(this.Rule, nameof(this.Rule)).IsNotNull();
            Ensure.Arg(this.TrueTask, nameof(this.TrueTask)).IsNotNull();

            if (this.FalseTask == null)
            {
                return new ConditionalTask<TContext>(
                    this.Rule,
                    this.TrueTask.Build());
            }
            else
            {
                return new ConditionalElseTask<TContext>(
                    this.Rule,
                    this.TrueTask.Build(),
                    this.FalseTask.Build());
            }
        }
    }
}
