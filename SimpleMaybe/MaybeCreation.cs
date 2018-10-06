namespace SimpleMaybe
{
    public static class Maybe
    {
        public static Maybe<T> None<T>()
        {
            return new Maybe<T>();
        }

        public static Maybe<T> Some<T>(T value)
        {
            return new Maybe<T>(value: value);
        }

        public static Maybe<T> ToSome<T>(this T value)
        {
            return Some(value);
        }

        public static Maybe<T> SomeOrNone<T>(T valueOrNull)
            where T : class
        {
            return valueOrNull == null
                ? None<T>()
                : Some(valueOrNull);
        }

        public static Maybe<T> ToSomeOrNone<T>(this T value)
            where T : class
        {
            return SomeOrNone(value);
        }

        public static Maybe<T> SomeOrNoneFromNullable<T>(T? value)
            where T : struct
        {
            return value == null
                ? None<T>()
                : Some(value.Value);
        }

        public static Maybe<T> ToSomeOrNoneFromNullable<T>(this T? value)
            where T : struct
        {
            return SomeOrNoneFromNullable(value);
        }
    }
}