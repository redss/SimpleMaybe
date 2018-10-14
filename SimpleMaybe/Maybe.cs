using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace SimpleMaybe
{
    public struct Maybe<TValue> : IEquatable<Maybe<TValue>>
    {
        private readonly bool _hasValue;
        private readonly TValue _value;

        internal Maybe(TValue value)
        {
            if (value == null)
            {
                throw new ArgumentException(
                    message: $"Value of Maybe.Some<{typeof(TValue).Name}> cannot be null.",
                    paramName: nameof(value)
                );
            }

            _hasValue = true;
            _value = value;
        }

        // enumeration

        [Pure]
        public IEnumerable<TValue> ToEnumerable()
        {
            if (_hasValue)
            {
                yield return _value;
            }
        }

        [Pure]
        public IEnumerator<TValue> GetEnumerator()
        {
            return ToEnumerable().GetEnumerator();
        }

        // access

        [Pure]
        public bool HasValue => _hasValue;

        [Pure]
        public TValue ValueOrDefault(TValue defaultValue = default(TValue))
        {
            return _hasValue
                ? _value
                : defaultValue;
        }

        [Pure]
        public TValue ValueOrFail()
        {
            return _hasValue
                ? _value
                : throw new InvalidOperationException("Cannot get value from None.");
        }

        // matching

        public async Task<TReturn> MatchAsync<TReturn>(Func<TValue, Task<TReturn>> some, Func<Task<TReturn>> none)
        {
            return await Match(some, none).ConfigureAwait(false);
        }

        public TReturn Match<TReturn>(Func<TValue, TReturn> some, Func<TReturn> none)
        {
            if (some == null) throw new ArgumentNullException(nameof(some));
            if (none == null) throw new ArgumentNullException(nameof(none));

            return _hasValue
                ? some(_value)
                : none();
        }

        public async Task MatchSomeAsync(Func<TValue, Task> some)
        {
            await MatchAsync(some, () => Task.CompletedTask);
        }

        public void MatchSome(Action<TValue> some)
        {
            Match(some, () => { });
        }

        public async Task MatchNoneAsync(Func<Task> none)
        {
            await MatchAsync(value => Task.CompletedTask, none);
        }

        public void MatchNone(Action none)
        {
            Match(value => {}, none);
        }

        public async Task MatchAsync(Func<TValue, Task> some, Func<Task> none)
        {
            await Match(some, none).ConfigureAwait(false);
        }

        public void Match(Action<TValue> some, Action none)
        {
            if (some == null) throw new ArgumentNullException(nameof(some));
            if (none == null) throw new ArgumentNullException(nameof(none));

            if (_hasValue)
            {
                some(_value);
            }
            else
            {
                none();
            }
        }

        // selecting

        // todo: handle these warnings

        [Pure]
        public Maybe<TReturn> Select<TReturn>(Func<TValue, TReturn> selector)
        {
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return Match(
                some: value => Maybe.Some(value: selector(arg: value)),
                none: () => Maybe.None<TReturn>()
            );
        }

        [Pure]
        public async Task<Maybe<TReturn>> SelectAsync<TReturn>(Func<TValue, Task<TReturn>> selector)
        {
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return await MatchAsync(
                some: async value => Maybe.Some(await selector(value).ConfigureAwait(false)),
                none: async () => Maybe.None<TReturn>()
            );
        }

        [Pure]
        public Maybe<TReturn> SelectMaybe<TReturn>(Func<TValue, Maybe<TReturn>> selector)
        {
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return Match(
                some: value => selector(value),
                none: () => Maybe.None<TReturn>()
            );
        }

        [Pure]
        public async Task<Maybe<TReturn>> SelectMaybeAsync<TReturn>(Func<TValue, Task<Maybe<TReturn>>> selector)
        {
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return await MatchAsync(
                some: async value => await selector(value).ConfigureAwait(false),
                none: async () => Maybe.None<TReturn>()
            );
        }

        // filtering

        public Maybe<TValue> Where(Func<TValue, bool> predicate)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            return SelectMaybe(value => predicate(value)
                ? Maybe.Some(value)
                : Maybe.None<TValue>()
            );
        }

        public async Task<Maybe<TValue>> WhereAsync(Func<TValue, Task<bool>> predicate)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            return await SelectMaybeAsync(async value => await predicate(value)
                ? Maybe.Some(value)
                : Maybe.None<TValue>()
            );
        }

        // equality

        public static bool operator ==(Maybe<TValue> left, Maybe<TValue> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Maybe<TValue> left, Maybe<TValue> right)
        {
            return !(left == right);
        }

        [Pure]
        public bool Equals(Maybe<TValue> other)
        {
            if (HasValue && other.HasValue)
            {
                return Equals(_value, other._value);
            }

            if (!HasValue && !other.HasValue)
            {
                return true;
            }

            return false;
        }

        [Pure]
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (GetType() != obj.GetType())
            {
                return false;
            }

            return Equals((Maybe<TValue>) obj);
        }

        [Pure]
        public override int GetHashCode()
        {
            return _value != null
                ? _value.GetHashCode()
                : 0;
        }

        // diagnostics

        [Pure]
        public override string ToString()
        {
            var typeName = typeof(TValue).Name;

            return _hasValue
                ? $"Some<{typeName}>({_value})"
                : $"None<{typeName}>()";
        }
    }
}
