using System;
using System.Linq;
using SimpleRouting.Routing;
using System.Threading.Tasks;
using SimpleRouting.Tests.SampleImplementations;
using Xunit;

namespace SimpleRouting.Tests
{
    public class TestRouting
    {
        private static void Times(int times, Action action)
        {
            foreach (var i in Enumerable.Range(0, times))
                action();
        }
        
        [Fact]
        public async Task TestEmptyRouting()
        {
            var router = new Router<BasicRoutingArgs<object>>();
            Assert.Equal(0, await router.RouteAsync(null!));
        }

        [Theory]
        [InlineData(6, 9, 3)]
        [InlineData(7, 10, 3)]
        [InlineData(8, 10, 2)]
        [InlineData(9, 10, 1)]
        [InlineData(10, 10, 0)]
        public async Task TestConditionalRouting(int input, int expectedResult, int expectedEligible)
        {
            const int incrementRoutes = 3;

            var router = new Router<BasicRoutingArgs<IntWrapper>>();
            Times(incrementRoutes, () => router.Add(new IncrementRoute()));

            var args = new BasicRoutingArgs<IntWrapper>(new IntWrapper(input));
            var routed = await router.RouteAsync(args);
            
            Assert.Equal(expectedEligible, routed);
            Assert.Equal(expectedResult, args.Data.Int);
        }
    }
}
