using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipelineFramework
{
    public sealed class SwitchPipelineTask<TContext> : IPipelineTask<TContext>
    {
        private readonly IEnumerable<ConditionalTask<TContext>> _cases;
        private readonly IPipelineTask<TContext> _default;

        public SwitchPipelineTask(TContext context, IEnumerable<ConditionalTask<TContext>> cases, IPipelineTask<TContext> @default = null)
        {
            this.Context = context;
            this._cases = cases;
            this._default = @default;
        }

        public TContext Context { get; }

        public async Task<IPipelineResult> ExecuteAsync()
        {
            try
            {
                var @case = this._cases.SingleOrDefault(c => c.Rule());

                if (this._default == null)
                {
                    throw new NotSupportedException("There is no default case");
                }

                return await @case.ExecuteAsync();
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException("Multiple case match the condition, this is not allowed", ex);
            }
        }
    }
}
