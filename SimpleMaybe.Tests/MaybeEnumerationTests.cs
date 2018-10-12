using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;

namespace SimpleMaybe.Tests
{
    public class MaybeEnumerationTests
    {
        [Test]
        public void some_can_be_enumerated_in_a_foreach_loop()
        {
            var some = Maybe.Some(10);

            var enumeratedValues = new List<int>();

            foreach (var value in some)
            {
                enumeratedValues.Add(value);
            }

            enumeratedValues.Should().Equal(10);
        }

        [Test]
        public void none_will_not_be_enumerated_in_foreach_loop()
        {
            var none = Maybe.None<int>();

            var enumeratedValues = new List<int>();

            foreach (var value in none)
            {
                enumeratedValues.Add(value);
            }

            enumeratedValues.Should().BeEmpty();
        }

        [Test]
        public void ienumerable_representation_of_some_contains_one_item()
        {
            Maybe.Some(10).ToEnumerable().Should().Equal(10);
        }

        [Test]
        public void ienumerable_representation_of_none_is_empty()
        {
            Maybe.None<int>().ToEnumerable().Should().BeEmpty();
        }
    }
}