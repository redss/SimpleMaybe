using System;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace SimpleMaybe.Tests
{
    public class MaybeFilteringTests
    {
        [Test]
        public void filtering_some_value_that_satisfies_predicate_results_in_same_value()
        {
            Maybe.Some(10)
                .Where(value => value > 0)
                .Should().Be(Maybe.Some(10));

            Maybe.Some(10)
                .WhereAsync(value => Task.FromResult(value > 0))
                .Result
                .Should().Be(Maybe.Some(10));
        }

        [Test]
        public void filtering_some_value_that_doesnt_satisfy_predicate_results_in_none()
        {
            Maybe.Some(10)
                .Where(value => value > 100)
                .Should().Be(Maybe.None<int>());

            Maybe.Some(10)
                .WhereAsync(value => Task.FromResult(value > 100))
                .Result
                .Should().Be(Maybe.None<int>());
        }

        [Test]
        public void filtering_none_results_in_none()
        {
            Maybe.None<int>()
                .Where(value => value > 0)
                .Should().Be(Maybe.None<int>());

            Maybe.None<int>()
                .WhereAsync(value => Task.FromResult(value > 0))
                .Result
                .Should().Be(Maybe.None<int>());
        }

        [TestCase(10)]
        [TestCase(null)]
        public void where_parameters_are_required(int? intOrNull)
        {
            var maybe = intOrNull.ToMaybe();

            ShouldRequireArgument(() => maybe.Where(null));
            ShouldRequireArgument(() => maybe.WhereAsync(null));
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