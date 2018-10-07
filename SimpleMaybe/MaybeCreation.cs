using System.Diagnostics.Contracts;

namespace SimpleMaybe
{
    [Pure]
    public static class Maybe
    {
        [Pure]
        public static Maybe<TValue> None<TValue>()
        {
            return new Maybe<TValue>();
        }

        [Pure]
        public static Maybe<TValue> Some<TValue>(TValue value)
        {
            return new Maybe<TValue>(value: value);
        }

        [Pure]
        public static Maybe<TValue> ToSome<TValue>(this TValue value)
        {
            return Some(value);
        }

        [Pure]
        public static Maybe<TValue> SomeOrNone<TValue>(TValue valueOrNull)
            where TValue : class
        {
            return valueOrNull == null
                ? None<TValue>()
                : Some(valueOrNull);
        }

        [Pure]
        public static Maybe<TValue> ToSomeOrNone<TValue>(this TValue value)
            where TValue : class
        {
            return SomeOrNone(value);
        }

        [Pure]
        public static Maybe<TValue> SomeOrNoneFromNullable<TValue>(TValue? value)
            where TValue : struct
        {
            return value == null
                ? None<TValue>()
                : Some(value.Value);
        }

        [Pure]
        public static Maybe<TValue> ToSomeOrNoneFromNullable<TValue>(this TValue? value)
            where TValue : struct
        {
            return SomeOrNoneFromNullable(value);
        }
    }
}