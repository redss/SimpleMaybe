using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

// ReSharper disable ReturnValueOfPureMethodIsNotUsed - we're testing for exceptions

namespace SimpleMaybe.Tests
{
    public class MaybeComparerTests
    {
        private MaybeComparer<int> _defaultComparer;

        [SetUp]
        public void set_up()
        {
            _defaultComparer = MaybeComparer<int>.Default;
        }

        [Test]
        public void can_compare_maybe_values()
        {
            _defaultComparer.Compare(Maybe.Some(20), Maybe.Some(10)).Should().BeGreaterThan(0);
            _defaultComparer.Compare(Maybe.Some(10), Maybe.Some(10)).Should().Be(0);
            _defaultComparer.Compare(Maybe.Some(10), Maybe.Some(20)).Should().BeLessThan(0);
        }

        [Test]
        public void some_is_always_greater_than_none()
        {
            _defaultComparer.Compare(Maybe.Some(10), Maybe.None<int>()).Should().BeGreaterThan(0);
            _defaultComparer.Compare(Maybe.None<int>(), Maybe.Some(10)).Should().BeLessThan(0);
        }

        [Test]
        public void two_nones_are_equal()
        {
            _defaultComparer.Compare(Maybe.None<int>(), Maybe.None<int>()).Should().Be(0);
        }
        
        [Test]
        public void can_sort_maybe_values()
        {
            var maybes = new[]
            {
                Maybe.Some(10),
                Maybe.Some(5),
                Maybe.None<int>(),
                Maybe.Some(20),
                Maybe.Some(15),
                Maybe.None<int>()
            };

            var result = maybes.OrderBy(maybe => maybe, MaybeComparer<int>.Default);

            result.Should().Equal(new[]
            {
                Maybe.None<int>(),
                Maybe.None<int>(),
                Maybe.Some(5),
                Maybe.Some(10),
                Maybe.Some(15),
                Maybe.Some(20)
            });
        }

        [Test]
        public void can_compare_using_custom_value_comparer()
        {
            var customComparer = new MaybeComparer<int>(
                valueComparer: new ReverseIntComparer()
            );

            customComparer.Compare(Maybe.Some(20), Maybe.Some(10)).Should().BeLessThan(0);
            customComparer.Compare(Maybe.Some(10), Maybe.Some(10)).Should().Be(0);
            customComparer.Compare(Maybe.Some(10), Maybe.Some(20)).Should().BeGreaterThan(0);
        }

        class ReverseIntComparer : IComparer<int>
        {
            public int Compare(int first, int second) => -first.CompareTo(second);
        }
    }
}