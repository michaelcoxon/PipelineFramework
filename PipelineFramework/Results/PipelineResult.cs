using System;

namespace PipelineFramework
{
    public class PipelineResult : IPipelineResult
    {
        public bool IsSuccess { get; }

        public Exception Exception { get; }

        public PipelineResult()
        {
            this.IsSuccess = true;
        }

        public PipelineResult(Exception exception)
        {
            this.Exception = exception;
            this.IsSuccess = false;
        }
    }

    public class PipelineResult<TResult> : PipelineResult
    {
        public TResult Result { get; }

        public PipelineResult(TResult result)
        {
            this.Result = result;
        }
    }
}