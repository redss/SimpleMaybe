﻿using System.Diagnostics.Contracts;

namespace SimpleMaybe
{
    public static class MaybeExtensions
    {
        [Pure]
        public static TValue? ToNullable<TValue>(this Maybe<TValue> maybe)
            where TValue : struct
        {
            return maybe.Match<TValue?>(
                some: value => value,
                none: () => null
            );
        }

        public static Maybe<TValue> SelectMaybe<TValue>(this Maybe<Maybe<TValue>> maybeOfMaybe)
        {
            return maybeOfMaybe.SelectMaybe(maybe => maybe);
        }
    }
}