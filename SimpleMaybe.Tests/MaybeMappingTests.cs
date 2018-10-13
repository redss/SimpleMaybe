using System;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

// ReSharper disable ReturnValueOfPureMethodIsNotUsed - we're testing for exceptions

namespace SimpleMaybe.Tests
{
    public class MaybeMappingTests
    {
        [Test]
        public void can_map_some_value()
        {
            Maybe.Some(10)
                .Map(value => value + 10)
                .Should().Be(Maybe.Some(20));

            Maybe.Some(10)
                .MapAsync(value => Task.FromResult(value + 10))
                .Result
                .Should().Be(Maybe.Some(20));
        }

        [Test]
        public void can_flat_map_some_value()
        {
            Maybe.Some(10)
                .FlatMap(value => Maybe.Some(value + 10))
                .Should().Be(Maybe.Some(20));

            Maybe.Some(10)
                .FlatMap(value => Maybe.None<int>())
                .Should().Be(Maybe.None<int>());

            Maybe.Some(10)
                .FlatMapAsync(value => Task.FromResult(Maybe.Some(value + 10)))
                .Result
                .Should().Be(Maybe.Some(20));

            Maybe.Some(10)
                .FlatMapAsync(value => Task.FromResult(Maybe.None<int>()))
                .Result
                .Should().Be(Maybe.None<int>());
        }

        [Test]
        public void none_will_not_be_mapped()
        {
            Maybe.None<int>()
                .Map(value => value + 10)
                .Should().Be(Maybe.None<int>());

            Maybe.None<int>()
                .MapAsync(value => Task.FromResult(value + 10))
                .Result
                .Should().Be(Maybe.None<int>());
        }

        [Test]
        public void can_flat_map_none()
        {
            Maybe.None<int>()
                .FlatMap(value => Maybe.Some(value + 10))
                .Should().Be(Maybe.None<int>());

            Maybe.None<int>()
                .FlatMap(value => Maybe.None<int>())
                .Should().Be(Maybe.None<int>());

            Maybe.None<int>()
                .FlatMapAsync(value => Task.FromResult(Maybe.Some(value + 10)))
                .Result
                .Should().Be(Maybe.None<int>());

            Maybe.None<int>()
                .FlatMapAsync(value => Task.FromResult(Maybe.None<int>()))
                .Result
                .Should().Be(Maybe.None<int>());
        }

        [TestCase(10)]
        [TestCase(null)]
        public void map_parameters_are_required(int? valueOrNull)
        {
            var maybe = valueOrNull.ToSomeOrNoneFromNullable();

            ShouldRequireArgument(() => maybe.Map<int>(null));
            ShouldRequireArgument(() => maybe.FlatMap<int>(null));

            ShouldRequireArgument(() => maybe.FlatMapAsync<int>(null));
            ShouldRequireArgument(() => maybe.FlatMapAsync<int>(null));
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