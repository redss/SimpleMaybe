using System;
using FluentAssertions;
using NUnit.Framework;

// ReSharper disable ReturnValueOfPureMethodIsNotUsed - we're testing for exceptions

namespace SimpleMaybe.Tests
{
    public class MaybeCreationTests
    {
        [Test]
        public void default_maybe_is_none()
        {
            default(Maybe<int>).Should().Be(Maybe.None<int>());
            new Maybe<int>().Should().Be(Maybe.None<int>());
        }

        [Test]
        public void can_create_some_using_an_extension_method()
        {
            10.ToSome().Should().Be(Maybe.Some(10));
            "aaa".ToSome().Should().Be(Maybe.Some("aaa"));
        }

        [Test]
        public void can_create_some_from_a_nullable_value()
        {
            int? nullableInt = 10;

            nullableInt.ToSome().Should().Be(Maybe.Some(10));
            Maybe.Some(nullableInt).Should().Be(Maybe.Some(10));
        }

        [Test]
        public void cannot_create_some_with_a_null_reference()
        {
            Action creatingSome = () => Maybe.Some<string>(null);

            creatingSome.Should().Throw<ArgumentNullException>()
                .Which.Message.Should().StartWith("Value of Maybe.Some<String> cannot be null.");
        }

        [Test]
        public void cannot_create_some_with_a_null_value()
        {
            Action creatingSomeNullable = () => Maybe.Some<int>(null);

            creatingSomeNullable.Should().Throw<ArgumentNullException>()
                .Which.Message.Should().StartWith("Value of Maybe.Some<Int32> cannot be null."); // todo: inform, that nullable was used?
        }

        [Test]
        public void can_create_maybe_from_reference_type()
        {
            Maybe.FromReference("aaa").Should().Be(Maybe.Some("aaa"));
            "aaa".ToMaybe().Should().Be(Maybe.Some("aaa"));

            Maybe.FromReference((string) null).Should().Be(Maybe.None<string>());
            ((string)null).ToMaybe().Should().Be(Maybe.None<string>());
        }

        [Test]
        public void can_create_maybe_from_nullable_type()
        {
            Maybe.FromNullable((int?) 10).Should().Be(Maybe.Some(10));
            ((int?)10).ToMaybe().Should().Be(Maybe.Some(10));

            Maybe.FromNullable((int?) null).Should().Be(Maybe.None<int>());
            ((int?)null).ToMaybe().Should().Be(Maybe.None<int>());
        }
    }
}