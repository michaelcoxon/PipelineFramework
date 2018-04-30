using System;

namespace PipelineFramework
{
    public sealed class ConditionalTaskResult : IPipelineResult
    {
        private readonly IPipelineResult _pipelineResult;
        private readonly ConditionalTaskResultEnum _result;

        public ConditionalTaskResult(IPipelineResult pipelineResult)
        {
            this._pipelineResult = pipelineResult;
            if (this._pipelineResult.IsSuccess)
            {
                this._result = ConditionalTaskResultEnum.Success;
            }
            else
            {
                this._result = ConditionalTaskResultEnum.Error;
            }
            this.Exception = pipelineResult.Exception;
        }

        public ConditionalTaskResult(ConditionalTaskResultEnum wasNotRun)
        {
            this._result = wasNotRun;
        }

        public bool IsSuccess => this._result != ConditionalTaskResultEnum.Error;

        public Exception Exception { get; }
    }
}