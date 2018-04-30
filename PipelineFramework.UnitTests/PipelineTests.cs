using System;
using System.Threading.Tasks;
using Xunit;

namespace PipelineFramework.UnitTests
{
    public class PipelineTests
    {
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
    }
}
