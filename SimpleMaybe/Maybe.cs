using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace SimpleMaybe
{
    public struct Maybe<TValue> : IEquatable<Maybe<TValue>>, IComparable<Maybe<TValue>>, IComparable
    {
        private readonly bool _hasValue;
        private readonly TValue _value;

        internal Maybe(TValue value)
        {
            if (value == null)
            {
                throw new InvalidOperationException($"Value of Maybe.Some<{typeof(TValue).Name}> cannot be null.");
            }

            _hasValue = true;
            _value = value;
        }

        // enumeration

        [Pure]
        public IEnumerable<TValue> AsEnumerable()
        {
            if (_hasValue)
            {
                yield return _value;
            }
        }

        [Pure]
        public IEnumerator<TValue> GetEnumerator()
        {
            return AsEnumerable().GetEnumerator();
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
            if (_hasValue)
            {
                some(_value);
            }
            else
            {
                none();
            }
        }

        // mapping

        [Pure]
        public Maybe<TReturn> Map<TReturn>(Func<TValue, TReturn> map)
        {
            return _hasValue
                ? Maybe.Some(map(_value))
                : Maybe.None<TReturn>();
        }

        [Pure]
        public async Task<Maybe<TReturn>> MapAsync<TReturn>(Func<TValue, Task<TReturn>> map)
        {
            return _hasValue
                ? Maybe.Some(await map(_value).ConfigureAwait(false))
                : Maybe.None<TReturn>();
        }

        [Pure]
        public Maybe<TReturn> FlatMap<TReturn>(Func<TValue, Maybe<TReturn>> map)
        {
            return _hasValue
                ? map(_value)
                : Maybe.None<TReturn>();
        }

        [Pure]
        public async Task<Maybe<TReturn>> FlatMapAsync<TReturn>(Func<TValue, Task<Maybe<TReturn>>> map)
        {
            return _hasValue
                ? await map(_value).ConfigureAwait(false)
                : Maybe.None<TReturn>();
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

        // comparison

        int IComparable.CompareTo(object obj)
        {
            if (ReferenceEquals(obj, null))
            {
                return 1;
            }

            if (obj is Maybe<TValue> maybe)
            {
                return CompareTo(maybe);
            }

            throw new InvalidOperationException($"Cannot compare {this} with object of type {obj.GetType().Name}.");
        }

        [Pure]
        public int CompareTo(Maybe<TValue> other)
        {
            if (HasValue && other.HasValue)
            {
                return Comparer<TValue>.Default.Compare(_value, other._value);
            }

            if (!HasValue && !other.HasValue)
            {
                return 0;
            }

            return HasValue ? 1 : -1;
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
