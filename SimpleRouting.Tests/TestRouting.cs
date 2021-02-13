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
            var router = new Router<BasicRoutingContext<object>>();
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

            var router = new Router<BasicRoutingContext<IntWrapper>>();
            Times(incrementRoutes, () => router.Add(new IncrementRoute()));

            var context = new BasicRoutingContext<IntWrapper>(new IntWrapper(input));
            var routed = await router.RouteAsync(context);
            
            Assert.Equal(expectedEligible, routed);
            Assert.Equal(expectedResult, context.Data.Int);
        }

        [Fact]
        public async Task TestShortCircuiting()
        {
            var router = new Router<BasicRoutingContext<IntWrapper>>()
            {
                new Router<BasicRoutingContext<IntWrapper>>()
                {
                    new IncrementRoute(), // hit
                    new ShortCircuitRoute(), // break, jump all 3 lines below
                    new IncrementRoute(),
                    new IncrementRoute(),
                    new IncrementRoute(),
                },
                new IncrementRoute() // here
            };

            var wrapper = new IntWrapper() {Int = 0};
            await router.RouteAsync(new BasicRoutingContext<IntWrapper>(wrapper));
            
            Assert.Equal(2, wrapper.Int);
        }

        [Fact]
        public async Task TestBreakRouting()
        {
            var router = new Router<BasicRoutingContext<IntWrapper>>()
            {
                new Router<BasicRoutingContext<IntWrapper>>()
                {
                    new Router<BasicRoutingContext<IntWrapper>>()
                    {
                        new IncrementRoute(), // hit
                        new IncrementRoute(), // hit
                        new StopRoute(), // break, stop all routing
                        new IncrementRoute()
                    },
                    new IncrementRoute(),
                },
                new IncrementRoute(),
                new IncrementRoute(),
                new IncrementRoute()
            };
            
            var wrapper = new IntWrapper() {Int = 0};
            await router.RouteAsync(new BasicRoutingContext<IntWrapper>(wrapper));
            
            Assert.Equal(2, wrapper.Int); // should be 2 instead of 7
        }
        
        [Fact]
        public async Task TestCombinedRouting()
        {
            var router = new Router<BasicRoutingContext<IntWrapper>>()
            {
                new Router<BasicRoutingContext<IntWrapper>>() // 1st route: ok
                {
                    new Router<BasicRoutingContext<IntWrapper>>()
                    {
                        new IncrementRoute(), // hit: 8
                        new IncrementRoute(), // hit: 9
                        new ShortCircuitRoute(), // skip 2 lines below
                        new IncrementRoute(),
                        new IncrementRoute()
                    },
                    new IncrementRoute(), // hit: 10
                    new IncrementRoute(), // miss: 10 is not eligible
                    new StopRoute(), // stop routing altogether
                    new IncrementRoute()
                },
                new IncrementRoute(),
                new IncrementRoute()
            };
            
            var wrapper = new IntWrapper() {Int = 7};
            var routed = await router.RouteAsync(new BasicRoutingContext<IntWrapper>(wrapper));
            
            Assert.Equal(10, wrapper.Int); // should be 10, see comments above
            Assert.Equal(1, routed);
        }
    }
}
