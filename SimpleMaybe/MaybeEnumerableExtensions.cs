using System.Collections.Generic;
using System.Linq;

namespace SimpleMaybe
{
    public static class MaybeEnumerableExtensions
    {
        public static IEnumerable<TValue> SomeValues<TValue>(this IEnumerable<Maybe<TValue>> maybes)
        {
            return maybes.SelectMany(maybe => maybe.ToEnumerable());
        }
    }
}