using FluentAssertions;
using NUnit.Framework;

// ReSharper disable SuspiciousTypeConversion.Global - that's what we're testing

namespace SimpleMaybe.Tests
{
    public class MaybeEqualityTests
    {
        [Test]
        public void maybe_values_are_equal_when_underlying_values_are_equal()
        {
            TestEquality(Maybe.Some(10), Maybe.Some(10), shouldBeEqual: true);
            TestEquality(Maybe.Some(10), Maybe.Some(12), shouldBeEqual: false);

            TestEquality(Maybe.Some("aaa"), Maybe.Some("aaa"), shouldBeEqual: true);
            TestEquality(Maybe.Some("aaa"), Maybe.Some("bbb"), shouldBeEqual: false);
        }

        [Test]
        public void some_is_never_equal_to_none()
        {
            TestEquality(Maybe.None<int>(), Maybe.Some(10), shouldBeEqual: false);
            TestEquality(Maybe.None<string>(), Maybe.Some("aaa"), shouldBeEqual: false);
        }

        [Test]
        public void two_nones_are_equal()
        {
            TestEquality(Maybe.None<int>(), Maybe.None<int>(), shouldBeEqual: true);
            TestEquality(Maybe.None<string>(), Maybe.None<string>(), shouldBeEqual: true);
        }

        private void TestEquality<TValue>(Maybe<TValue> first, Maybe<TValue> second, bool shouldBeEqual)
        {
            first.Equals(second).Should().Be(shouldBeEqual);
            second.Equals(first).Should().Be(shouldBeEqual);

            (first == second).Should().Be(shouldBeEqual);
            (second == first).Should().Be(shouldBeEqual);

            (first != second).Should().Be(!shouldBeEqual);
            (second != first).Should().Be(!shouldBeEqual);

            ((object) first).Equals(second).Should().Be(shouldBeEqual);
            ((object) second).Equals(first).Should().Be(shouldBeEqual);
        }

        [Test]
        public void maybe_is_never_equal_to_null()
        {
            AnyMaybeShouldNotBeEqualTo(null);
        }

        [Test]
        public void maybe_is_never_equal_to_object_of_different_type()
        {
            AnyMaybeShouldNotBeEqualTo(10);
            AnyMaybeShouldNotBeEqualTo("aaa");
            AnyMaybeShouldNotBeEqualTo(new object());
        }

        private void AnyMaybeShouldNotBeEqualTo(object obj)
        {
            Maybe.Some(10).Equals(obj).Should().BeFalse();
            Maybe.None<int>().Equals(obj).Should().BeFalse();
            Maybe.Some("aaa").Equals(obj).Should().BeFalse();
            Maybe.None<string>().Equals(obj).Should().BeFalse();
        }

        [Test]
        public void hash_code_of_some_is_hash_code_of_contained_value()
        {
            SomeOfValueShouldHaveSameHashCodeAsValue(10);
            SomeOfValueShouldHaveSameHashCodeAsValue("aaa");
            SomeOfValueShouldHaveSameHashCodeAsValue(new object());
        }

        [Test]
        public void hash_code_of_none_equals_zero()
        {
            Maybe.None<int>().GetHashCode().Should().Be(0);
            Maybe.None<string>().GetHashCode().Should().Be(0);
            Maybe.None<object>().GetHashCode().Should().Be(0);
        }

        private void SomeOfValueShouldHaveSameHashCodeAsValue<TValue>(TValue value)
        {
            Maybe.Some(value).GetHashCode().Should().Be(value.GetHashCode());
        }
    }
}