using System;
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

        [TestCase(10)]
        [TestCase(null)]
        public void match_parameters_are_required(int? valueOrNull)
        {
            var maybe = valueOrNull.ToSomeOrNoneFromNullable();

            ShouldRequireArgument(() => maybe.Match(null, () => 0));
            ShouldRequireArgument(() => maybe.Match(value => 0, null));

            ShouldRequireArgument(() => maybe.Match(null, () => { }));
            ShouldRequireArgument(() => maybe.Match(value => { }, null));

            ShouldRequireArgument(() => maybe.MatchSome(null));
            ShouldRequireArgument(() => maybe.MatchNone(null));

            ShouldRequireArgument(() => maybe.MatchAsync(null, () => Task.FromResult(0)));
            ShouldRequireArgument(() => maybe.MatchAsync(value => Task.FromResult(0), null));

            ShouldRequireArgument(() => maybe.MatchAsync(null, () => Task.CompletedTask));
            ShouldRequireArgument(() => maybe.MatchAsync(value => Task.CompletedTask, null));

            ShouldRequireArgument(() => maybe.MatchSomeAsync(null));
            ShouldRequireArgument(() => maybe.MatchNoneAsync(null));
        }

        private void ShouldRequireArgument(Action action)
        {
            action.Should().Throw<ArgumentNullException>();
        }

        private void ShouldRequireArgument(Func<Task> action)
        {
            action.Should().Throw<ArgumentNullException>();
        }
    }
}