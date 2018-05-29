using EnsureFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipelineFramework
{
    public class SwitchPipelineTaskBuilder<TContext> : IPipelineTaskBuilder<TContext>
    {
        public ICollection<ConditionalPipelineTaskBuilder<SwitchPipelineTaskBuilder<TContext>, TContext, TContext>> Cases { get; }
        public IPipelineTaskBuilder<TContext> DefaultPipelineTask { get; set; }

        public SwitchPipelineTaskBuilder()
        {
            this.Cases = new List<ConditionalPipelineTaskBuilder<SwitchPipelineTaskBuilder<TContext>, TContext, TContext>>();
        }

        public IPipelineTask<TContext> Build()
        {
            Ensure.Arg(this.Cases, nameof(this.Cases)).IsNotNullOrEmpty();

            return new SwitchPipelineTask<TContext>(
                this.Cases.Select(b => (ConditionalPipelineTask<TContext>)b.Build()),
                this.DefaultPipelineTask.Build());
        }
    }
}
