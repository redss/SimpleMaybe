using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace SimpleMaybe.Tests
{
    public class MaybeMatchingTests
    {
        [Test]
        public void can_match_and_map_maybe_value()
        {
            Maybe.Some(10)
                .Match(
                    some: value => 20,
                    none: () => 0)
                .Should().Be(20);

            Maybe.Some(10)
                .MatchAsync(
                    some: value => Task.FromResult(20),
                    none: () => Task.FromResult(0))
                .Result
                .Should().Be(20);

            Maybe.None<int>()
                .Match(
                    some: value => 20,
                    none: () => 0)
                .Should().Be(0);

            Maybe.None<int>()
                .MatchAsync(
                    some: value => Task.FromResult(20),
                    none: () => Task.FromResult(0))
                .Result
                .Should().Be(0);
        }

        [Test]
        public void can_match_some_to_action()
        {
            var someCount = 0;
            var noneCount = 0;

            Maybe.Some(10)
                .Match(
                    some: value => { someCount++; }, 
                    none: () => { noneCount++; });

            Maybe.Some(10)
                .MatchAsync(
                    some: value => { someCount++; return Task.CompletedTask; },
                    none: () => { noneCount++; return Task.CompletedTask; })
                .Wait();

            Maybe.Some(10)
                .MatchSome(value => { someCount++; });

            Maybe.Some(10)
                .MatchSomeAsync(value => { someCount++; return Task.CompletedTask; })
                .Wait();

            someCount.Should().Be(4);
            noneCount.Should().Be(0);
        }

        [Test]
        public void can_match_none_to_action()
        {
            var someCount = 0;
            var noneCount = 0;

            Maybe.None<int>()
                .Match(
                    some: value => { someCount++; },
                    none: () => { noneCount++; });

            Maybe.None<int>()
                .MatchAsync(
                    some: value => { someCount++; return Task.CompletedTask; },
                    none: () => { noneCount++; return Task.CompletedTask; })
                .Wait();

            Maybe.None<int>()
                .MatchNone(() => { noneCount++; });

            Maybe.None<int>()
                .MatchNoneAsync(() => { noneCount++; return Task.CompletedTask; })
                .Wait();

            someCount.Should().Be(0);
            noneCount.Should().Be(4);
        }
    }
}