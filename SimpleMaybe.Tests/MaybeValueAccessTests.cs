using System;
using FluentAssertions;
using NUnit.Framework;

// ReSharper disable ReturnValueOfPureMethodIsNotUsed - we're testing for exceptions

namespace SimpleMaybe.Tests
{
    public class MaybeValueAccessTests
    {
        [Test]
        public void can_check_whether_maybe_has_value()
        {
            Maybe.Some(10).HasValue.Should().BeTrue();
            Maybe.None<int>().HasValue.Should().BeFalse();
        }

        [Test]
        public void can_always_get_value_from_some()
        {
            var some = Maybe.Some(10);

            some.ValueOrFail().Should().Be(10);
            some.ValueOrDefault().Should().Be(10);
        }

        [Test]
        public void getting_value_from_none_will_fail()
        {
            Action gettingValue = () => Maybe.None<int>().ValueOrFail();

            gettingValue.Should().Throw<InvalidOperationException>();
        }

        [Test]
        public void can_get_default_value_from_none()
        {
            Maybe.None<int>().ValueOrDefault().Should().Be(0);
            Maybe.None<string>().ValueOrDefault().Should().Be(null);

            Maybe.None<int>().ValueOrDefault(10).Should().Be(10);
            Maybe.None<string>().ValueOrDefault("aaa").Should().Be("aaa");
        }

        [Test]
        public void can_get_nullable_value_from_maybe()
        {
            Maybe.Some(10).ToNullable().Should().Be(10);
            Maybe.None<int>().ToNullable().Should().BeNull();
        }
    }
}
