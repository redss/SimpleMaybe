using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace SimpleMaybe
{
    public class MaybeComparer<TValue> : IComparer<Maybe<TValue>>
    {
        public static MaybeComparer<TValue> Default { get; } = new MaybeComparer<TValue>(Comparer<TValue>.Default);

        private readonly IComparer<TValue> _valueComparer;

        public MaybeComparer(IComparer<TValue> valueComparer)
        {
            _valueComparer = valueComparer ?? throw new ArgumentNullException(nameof(valueComparer));
        }

        [Pure]
        public int Compare(Maybe<TValue> first, Maybe<TValue> second)
        {
            return first.Match(
                some: firstValue => second.Match(
                    some: secondValue => _valueComparer.Compare(firstValue, secondValue),
                    none: () => 1
                ),
                none: () => second.Match(
                    some: secondValue => -1,
                    none: () => 0
                )
            );
        }
    }
}