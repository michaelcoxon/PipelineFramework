using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PipelineFramework
{
    public sealed class ForEachTask<TContext, TElement> : IPipelineTask<TContext>
    {
        private class ForEachTaskContext : IForEachTaskContext<TContext, TElement>
        {
            public TContext Context { get; set; }
            public TElement Value { get; set; }
        }

        private readonly Func<TContext, IEnumerable<TElement>> _selector;
        private readonly IPipelineTask<IForEachTaskContext<TContext, TElement>> _task;

        public ForEachTask(Func<TContext, IEnumerable<TElement>> selector, IPipelineTask<IForEachTaskContext<TContext, TElement>> task)
        {
            this._selector = selector;
            this._task = task;
        }

        public async Task<IPipelineResult> ExecuteAsync(TContext context)
        {
            var results = new List<IPipelineResult>();
            var items = this._selector(context).Select(i => new ForEachTaskContext
            {
                Context = context,
                Value = i,
            });

            foreach (var item in items)
            {
                results.Add(await this._task.ExecuteAsync(item));
            }

            return new AggregatePipelineResult(results);
        }
    }
}
