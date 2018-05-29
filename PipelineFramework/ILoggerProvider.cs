using Microsoft.Extensions.Logging;

namespace PipelineFramework
{
    public interface ILoggerProvider
    {
        ILogger GetLogger(string name);
        ILogger GetLogger<T>();
    }
}