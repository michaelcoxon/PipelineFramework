using System;
using System.Collections.Generic;
using System.Linq;

namespace PipelineFramework
{
    public sealed class AggregatePipelineResult : IPipelineResult
    {
        public IEnumerable<IPipelineResult> Results { get; }

        public bool IsSuccess { get; }

        public Exception Exception { get; }

        public AggregatePipelineResult(IEnumerable<IPipelineResult> results)
        {
            this.Results = results;
            this.IsSuccess = results.All(r => r.IsSuccess);

            if (!this.IsSuccess)
            {
                this.Exception = new AggregateException(results.Select(r => r.Exception));
            }
        }
    }
}