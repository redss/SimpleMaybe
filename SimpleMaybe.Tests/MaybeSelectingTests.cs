using System;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

// ReSharper disable ReturnValueOfPureMethodIsNotUsed - we're testing for exceptions

namespace SimpleMaybe.Tests
{
    public class MaybeSelectingTests
    {
        [Test]
        public void can_select_some_value()
        {
            Maybe.Some(10)
                .Select(value => value + 10)
                .Should().Be(Maybe.Some(20));

            Maybe.Some(10)
                .SelectAsync(value => Task.FromResult(value + 10))
                .Result
                .Should().Be(Maybe.Some(20));
        }

        [Test]
        public void can_select_to_maybe_and_flatten_some_value()
        {
            Maybe.Some(10)
                .SelectMaybe(value => Maybe.Some(value + 10))
                .Should().Be(Maybe.Some(20));

            Maybe.Some(10)
                .SelectMaybe(value => Maybe.None<int>())
                .Should().Be(Maybe.None<int>());

            Maybe.Some(10)
                .SelectMaybeAsync(value => Task.FromResult(Maybe.Some(value + 10)))
                .Result
                .Should().Be(Maybe.Some(20));

            Maybe.Some(10)
                .SelectMaybeAsync(value => Task.FromResult(Maybe.None<int>()))
                .Result
                .Should().Be(Maybe.None<int>());
        }

        [Test]
        public void selecting_none_will_result_in_none()
        {
            Maybe.None<int>()
                .Select(value => value + 10)
                .Should().Be(Maybe.None<int>());

            Maybe.None<int>()
                .SelectAsync(value => Task.FromResult(value + 10))
                .Result
                .Should().Be(Maybe.None<int>());
        }

        [Test]
        public void selecting_to_maybe_will_result_in_none()
        {
            Maybe.None<int>()
                .SelectMaybe(value => Maybe.Some(value + 10))
                .Should().Be(Maybe.None<int>());

            Maybe.None<int>()
                .SelectMaybe(value => Maybe.None<int>())
                .Should().Be(Maybe.None<int>());

            Maybe.None<int>()
                .SelectMaybeAsync(value => Task.FromResult(Maybe.Some(value + 10)))
                .Result
                .Should().Be(Maybe.None<int>());

            Maybe.None<int>()
                .SelectMaybeAsync(value => Task.FromResult(Maybe.None<int>()))
                .Result
                .Should().Be(Maybe.None<int>());
        }

        [TestCase(10)]
        [TestCase(null)]
        public void select_parameters_are_required(int? valueOrNull)
        {
            var maybe = valueOrNull.ToSomeOrNoneFromNullable();

            ShouldRequireArgument(() => maybe.Select<int>(null));
            ShouldRequireArgument(() => maybe.SelectMaybe<int>(null));

            ShouldRequireArgument(() => maybe.SelectMaybeAsync<int>(null));
            ShouldRequireArgument(() => maybe.SelectMaybeAsync<int>(null));
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