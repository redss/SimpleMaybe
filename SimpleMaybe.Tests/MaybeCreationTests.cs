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
        }

        [Test]
        public void can_create_some_using_an_extension_method()
        {
            10.ToSome().Should().Be(Maybe.Some(10));
        }

        [Test]
        public void cannot_create_some_with_a_null_value()
        {
            Action creatingSome = () => Maybe.Some<string>(null);
            creatingSome.Should().Throw<InvalidOperationException>();

            Action creatingSomeNullable = () => Maybe.Some<int?>(null);
            creatingSomeNullable.Should().Throw<InvalidOperationException>();

            // todo: assert messages
        }

        [Test]
        public void can_create_maybe_from_reference_type()
        {
            Maybe.SomeOrNone("aaa").Should().Be(Maybe.Some("aaa"));
            Maybe.SomeOrNone((string) null).Should().Be(Maybe.None<string>());
        }

        [Test]
        public void can_create_maybe_from_reference_type_using_an_extension_method()
        {
            "aaa".ToSomeOrNone().Should().Be(Maybe.Some("aaa"));
            ((string)null).ToSomeOrNone().Should().Be(Maybe.None<string>());
        }

        [Test]
        public void can_create_maybe_from_nullable_type()
        {
            Maybe.SomeOrNoneFromNullable((int?) 10).Should().Be(Maybe.Some(10));
            Maybe.SomeOrNoneFromNullable((int?) null).Should().Be(Maybe.None<int>());
        }

        [Test]
        public void can_create_maybe_from_nullable_type_using_an_extension_method()
        {
            ((int?)10).ToSomeOrNoneFromNullable().Should().Be(Maybe.Some(10));
            ((int?)null).ToSomeOrNoneFromNullable().Should().Be(Maybe.None<int>());
        }
    }
}