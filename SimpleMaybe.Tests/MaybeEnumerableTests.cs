using FluentAssertions;
using NUnit.Framework;

namespace SimpleMaybe.Tests
{
    public class MaybeEnumerableTests
    {
        [Test]
        public void can_get_all_some_values_from_enumeration_of_maybe()
        {
            var maybes = new[]
            {
                Maybe.Some(10),
                Maybe.None<int>(),
                Maybe.Some(20),
                Maybe.None<int>(),
                Maybe.Some(30),
                Maybe.None<int>()
            };

            maybes.SomeValues().Should().Equal(10, 20, 30);
        }
    }
}