using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipelineFramework
{
    public class LoggerProvider : ILoggerProvider
    {
        private class DisposeableLogger : IDisposeableLogger
        {
            private ILogger _logger;

            public DisposeableLogger(ILogger logger)
            {
                this._logger = logger;
            }

            public IDisposable BeginScope<TState>(TState state)
            {
                return this._logger.BeginScope(state);
            }

            public bool IsEnabled(LogLevel logLevel)
            {
                return this._logger.IsEnabled(logLevel);
            }

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
            {
                this._logger.Log(logLevel, eventId, state, exception, formatter);
            }

            public void Dispose()
            {
                if (this._logger is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }
        }

        private readonly ILoggerFactory _loggerFactory;

        static LoggerProvider()
        {
            SetCurrentLoggerProvider(new LoggerProvider(new LoggerFactory()));
        }

        public LoggerProvider(ILoggerFactory loggerFactory)
        {
            this._loggerFactory = loggerFactory;
        }

        public ILogger GetLogger<T>()
        {
            return this._loggerFactory.CreateLogger<T>();
        }

        public ILogger GetLogger(string name)
        {
            return this._loggerFactory.CreateLogger(name);
        }

        public static ILoggerProvider Current { get; private set; }

        public static void SetCurrentLoggerProvider(ILoggerProvider loggerProvider)
        {
            Current = loggerProvider;
        }

        public static IDisposeableLogger Create<T>()
        {
            return new DisposeableLogger(LoggerProvider.Current.GetLogger<T>());
        }

        public static IDisposeableLogger Create(string name)
        {
            return new DisposeableLogger(LoggerProvider.Current.GetLogger(name));
        }
    }

    public interface IDisposeableLogger : ILogger, IDisposable { }
}
