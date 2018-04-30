using System;

namespace PipelineFramework
{
    public interface IPipelineResult
    {
        bool IsSuccess { get; }
        Exception Exception { get; }

    }
}