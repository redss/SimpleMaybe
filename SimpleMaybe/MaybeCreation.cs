using System;
using System.Diagnostics.Contracts;

namespace SimpleMaybe
{
    [Pure]
    public static class Maybe
    {
        // none

        [Pure]
        public static Maybe<TValue> None<TValue>()
        {
            return new Maybe<TValue>();
        }

        // some

        [Pure]
        public static Maybe<TValue> ToSome<TValue>(this TValue value)
        {
            return Some(value);
        }

        [Pure]
        public static Maybe<TValue> Some<TValue>(TValue value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(
                    paramName: nameof(value),
                    message: $"Value of Maybe.Some<{typeof(TValue).Name}> cannot be null."
                );
            }

            return new Maybe<TValue>(value);
        }

        // some from nullable

        [Pure]
        public static Maybe<TValue> ToSome<TValue>(this TValue? value)
            where TValue : struct
        {
            return Some(value);
        }

        [Pure]
        public static Maybe<TValue> Some<TValue>(TValue? value)
            where TValue : struct
        {
            if (value == null)
            {
                throw new ArgumentNullException(
                    paramName: nameof(value),
                    message: $"Value of Maybe.Some<{typeof(TValue).Name}> cannot be null."
                );
            }

            return new Maybe<TValue>(value.Value);
        }

        // some or none from reference type

        [Pure]
        public static Maybe<TValue> FromReference<TValue>(TValue valueOrNull)
            where TValue : class
        {
            return valueOrNull == null
                ? None<TValue>()
                : Some(valueOrNull);
        }

        [Pure]
        public static Maybe<TValue> ToMaybe<TValue>(this TValue valueOrNull)
            where TValue : class
        {
            return FromReference(valueOrNull);
        }

        // some or none from nullable
        
        [Pure]
        public static Maybe<TValue> FromNullable<TValue>(TValue? value)
            where TValue : struct
        {
            return value == null
                ? None<TValue>()
                : Some(value.Value);
        }

        [Pure]
        public static Maybe<TValue> ToMaybe<TValue>(this TValue? value)
            where TValue : struct
        {
            return FromNullable(value);
        }
    }
}