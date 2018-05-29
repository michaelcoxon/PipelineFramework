using System;
using System.Threading.Tasks;
using Xunit;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace PipelineFramework.UnitTests
{
    public class PipelineTests
    {
        private class Wrapper<T>
        {
            public T Value { get; set; }
        }

        public PipelineTests()
        {
            LoggerProvider.SetCurrentLoggerProvider(
                new LoggerProvider(
                    new LoggerFactory()
                        .AddConsole(LogLevel.Trace, true)
                        .AddDebug(LogLevel.Trace)
                )
            );
        }

        [Fact]
        public async Task SimpleTest()
        {
            var pipeline = new Pipeline<string>();

            string shouldBeHelloWorld = null;

            pipeline.EnqueueTask(new ActionPipelineTask<string>((str) => shouldBeHelloWorld = str));

            var results = await pipeline.ExecuteAsync("Hello world");

            var expected = "Hello world";

            Assert.Equal(expected, shouldBeHelloWorld);
        }

        [Fact]
        public async Task SimpleExceptionTest()
        {
            var pipeline = new Pipeline<string>();

            pipeline.EnqueueTask(new ActionPipelineTask<string>((str) => throw new Exception()));

            var results = await pipeline.ExecuteAsync("Hello world");

            Assert.NotNull(results.Exception);
        }

        [Fact]
        public async Task BuildSimpleTaskTreeWithCondition()
        {
            var builder = new PipelineTaskBuilder<Wrapper<string>>();

            var pipeline = builder
                .BeginPipeline()
                    .If(ctx => ctx.Value.StartsWith('h'))
                        .RunAction(ctx => ctx.Value = string.Concat((new[] { char.ToUpper(ctx.Value.First()) }).Concat(ctx.Value.Skip(1).ToArray())))
                    .EndIf()
                .EndPipeline()
                .Build();

            {
                var context = new Wrapper<string>
                {
                    Value = "helLO wOrld!"
                };
                var result = await pipeline.ExecuteAsync(context);

                Assert.Equal("HelLO wOrld!", context.Value);
            }
            {
                var context = new Wrapper<string>
                {
                    Value = "HelLO wOrld!"
                };
                var result = await pipeline.ExecuteAsync(context);

                Assert.Equal("HelLO wOrld!", context.Value);
            }
        }

        [Fact]
        public async Task BuildSimpleTaskTreeWithConditionElse()
        {
            var builder = new PipelineTaskBuilder<Wrapper<string>>();

            var pipeline = builder
                .BeginPipeline()
                    .If(ctx => ctx.Value.StartsWith('h'))
                        .RunAction(ctx => ctx.Value = string.Concat((new[] { char.ToUpper(ctx.Value.First()) }).Concat(ctx.Value.Skip(1).ToArray())))
                    .Else()
                        .RunAction(ctx => ctx.Value = string.Concat((new[] { char.ToLower(ctx.Value.First()) }).Concat(ctx.Value.Skip(1).ToArray())))
                    .EndIf()
                .EndPipeline()
                .Build();
            {
                var context = new Wrapper<string>
                {
                    Value = "HelLO wOrld!"
                };

                var result = await pipeline.ExecuteAsync(context);

                Assert.Equal("helLO wOrld!", context.Value);
            }
            {
                var context = new Wrapper<string>
                {
                    Value = "helLO wOrld!"
                };

                var result = await pipeline.ExecuteAsync(context);

                Assert.Equal("HelLO wOrld!", context.Value);
            }
        }

        [Fact]
        public async Task BuildSimpleTaskTreeWithForEachAndConditionElse()
        {
            var builder = new PipelineTaskBuilder<Wrapper<Wrapper<string>[]>>();

            var pipeline = builder
                .BeginPipeline()
                    .ForEach(ctx => ctx.Value)
                        .If(ctx => ctx.Value.Value.StartsWith('h'))
                            .RunAction(ctx => ctx.Value.Value = string.Concat((new[] { char.ToUpper(ctx.Value.Value.First()) }).Concat(ctx.Value.Value.Skip(1).ToArray())))
                        .Else()
                            .RunAction(ctx => ctx.Value.Value = string.Concat((new[] { char.ToLower(ctx.Value.Value.First()) }).Concat(ctx.Value.Value.Skip(1).ToArray())))
                        .EndIf()
                    .EndForEach()
                .EndPipeline()
                .Build();
            {
                var context = new Wrapper<Wrapper<string>[]>
                {
                    Value = new[]
                    {
                        new Wrapper<string>{Value= "HelLO wOrld!" },
                        new Wrapper<string>{Value= "hellO wOrld!" },
                        new Wrapper<string>{Value= "HelLO wOrld!" },
                        new Wrapper<string>{Value= "helLO w0rld!" },
                    }
                };

                var result = await pipeline.ExecuteAsync(context);

                Assert.Equal("helLO wOrld!", context.Value[0].Value);
                Assert.Equal("HellO wOrld!", context.Value[1].Value);
                Assert.Equal("helLO wOrld!", context.Value[2].Value);
                Assert.Equal("HelLO w0rld!", context.Value[3].Value);
            }
        }

        [Fact]
        public async Task BuildSimpleTaskTreeWithNestedForEachAndConditionElse()
        {
            var builder = new PipelineTaskBuilder<Wrapper<Wrapper<Wrapper<string>[]>[]>>();

            var pipeline = builder
                .BeginPipeline()
                    .ForEach(ctx => ctx.Value)
                        .ForEach(ctx => ctx.Value.Value)
                            .If(ctx => ctx.Value.Value.StartsWith('h'))
                                .RunAction(ctx => ctx.Value.Value = string.Concat((new[] { char.ToUpper(ctx.Value.Value.First()) }).Concat(ctx.Value.Value.Skip(1).ToArray())))
                            .Else()
                                .RunAction(ctx => ctx.Value.Value = string.Concat((new[] { char.ToLower(ctx.Value.Value.First()) }).Concat(ctx.Value.Value.Skip(1).ToArray())))
                            .EndIf()
                        .EndForEach()
                    .EndForEach()
                .EndPipeline()
                .Build();
            {
                var context = new Wrapper<Wrapper<Wrapper<string>[]>[]>
                {
                    Value = new[]
                    {
                        new Wrapper<Wrapper<string>[]>
                        {
                            Value = new[]
                            {
                                new Wrapper<string>{Value= "HelLO wOrld!" },
                            }
                        },
                         new Wrapper<Wrapper<string>[]>
                        {
                            Value = new[]
                            {
                                new Wrapper<string>{Value= "hellO wOrld!" },
                            }
                        },
                          new Wrapper<Wrapper<string>[]>
                        {
                            Value = new[]
                            {
                                new Wrapper<string>{Value= "HelLO wOrld!" },
                            }
                        },
                           new Wrapper<Wrapper<string>[]>
                        {
                            Value = new[]
                            {
                                new Wrapper<string>{Value= "helLO w0rld!" },
                            }
                        }
                    }
                };

                var result = await pipeline.ExecuteAsync(context);

                Assert.Equal("helLO wOrld!", context.Value[0].Value[0].Value);
                Assert.Equal("HellO wOrld!", context.Value[1].Value[0].Value);
                Assert.Equal("helLO wOrld!", context.Value[2].Value[0].Value);
                Assert.Equal("HelLO w0rld!", context.Value[3].Value[0].Value);
            }
        }
    }
}
