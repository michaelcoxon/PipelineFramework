using System;
using System.Threading.Tasks;
using Xunit;
using System.Linq;

namespace PipelineFramework.UnitTests
{
    public class PipelineTests
    {
        private class Wrapper<T>
        {
            public T Value { get; set; }
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
                        .BeginPipeline()
                            .RunAction(ctx => ctx.Value = string.Concat((new[] { char.ToUpper(ctx.Value.First()) }).Concat(ctx.Value.Skip(1).ToArray())))
                        .EndPipeline()
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
                        .BeginPipeline()
                            .RunAction(ctx => ctx.Value = string.Concat((new[] { char.ToUpper(ctx.Value.First()) }).Concat(ctx.Value.Skip(1).ToArray())))
                        .EndPipeline()
                    .Else()
                        .BeginPipeline()
                            .RunAction(ctx => ctx.Value = string.Concat((new[] { char.ToLower(ctx.Value.First()) }).Concat(ctx.Value.Skip(1).ToArray())))
                        .EndPipeline()
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
    }
}
