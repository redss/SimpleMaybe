using System;
using FluentAssertions;
using NUnit.Framework;

// ReSharper disable ReturnValueOfPureMethodIsNotUsed - we're testing for exceptions

namespace SimpleMaybe.Tests
{
    public class MaybeComparisonTests
    {
        [Test]
        public void can_compare_maybe_values()
        {
            Maybe.Some(20).CompareTo(Maybe.Some(10)).Should().Be(1);
            Maybe.Some(10).CompareTo(Maybe.Some(10)).Should().Be(0);
            Maybe.Some(10).CompareTo(Maybe.Some(20)).Should().Be(-1);
        }

        [Test]
        public void some_is_always_greater_than_none()
        {
            Maybe.Some(10).CompareTo(Maybe.None<int>()).Should().Be(1);
            Maybe.None<int>().CompareTo(Maybe.Some(10)).Should().Be(-1);
        }

        [Test]
        public void two_nones_are_equal()
        {
            Maybe.None<int>().CompareTo(Maybe.None<int>()).Should().Be(0);
        }

        [Test]
        public void cannot_compare_uncomparable_types()
        {
            Action comparing = () => Maybe.Some(new object()).CompareTo(Maybe.Some(new object()));

            comparing.Should().Throw<Exception>();
        }

        [Test]
        public void maybe_is_always_greater_than_null()
        {
            ((IComparable)Maybe.Some(10)).CompareTo(null).Should().Be(1);
            ((IComparable)Maybe.None<int>()).CompareTo(null).Should().Be(1);
        }

        [Test]
        public void cannot_compare_maybe_with_different_type()
        {
            Action comparing = () => ((IComparable)Maybe.Some(10)).CompareTo(new object());

            comparing.Should().Throw<InvalidOperationException>();
        }
    }
}