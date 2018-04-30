using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipelineFramework
{
    public class ActionPipelineTask<TContext> : IPipelineTask<TContext>
    {
        private readonly Action<TContext> _action;

        public ActionPipelineTask(Action<TContext> action)
        {
            this._action = action;
        }

        public Task<IPipelineResult> ExecuteAsync(TContext context)
        {
            return Task.Run<IPipelineResult>(() =>
            {
                try
                {
                    this._action(context);
                    return new PipelineResult();
                }
                catch (Exception ex)
                {
                    return new PipelineResult(ex);
                }
            });
        }
    }
}
