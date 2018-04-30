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

        public SwitchPipelineTask(IEnumerable<ConditionalTask<TContext>> cases, IPipelineTask<TContext> @default = null)
        {
            this._cases = cases;
            this._default = @default;
        }

        public async Task<IPipelineResult> ExecuteAsync(TContext context)
        {
            try
            {
                var @case = this._cases.SingleOrDefault(c => c.Rule(context));

                if (this._default == null)
                {
                    throw new NotSupportedException("There is no default case");
                }

                return await @case.ExecuteAsync(context);
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException("Multiple case match the condition, this is not allowed", ex);
            }
        }
    }
}
