using System;
using System.Collections.Generic;
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
                throw new InvalidOperationException("Value cannot be null"); // todo: better message
            }

            _hasValue = true;
            _value = value;
        }

        // enumeration

        public IEnumerable<TValue> AsEnumerable()
        {
            if (_hasValue)
            {
                yield return _value;
            }
        }

        public IEnumerator<TValue> GetEnumerator()
        {
            return AsEnumerable().GetEnumerator();
        }

        // access

        public bool HasValue => _hasValue;

        public TValue ValueOrDefault(TValue defaultValue = default(TValue))
        {
            return _hasValue
                ? _value
                : defaultValue;
        }

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

        public Maybe<TReturn> Map<TReturn>(Func<TValue, TReturn> map)
        {
            return _hasValue
                ? Maybe.Some(map(_value))
                : Maybe.None<TReturn>();
        }

        public async Task<Maybe<TReturn>> MapAsync<TReturn>(Func<TValue, Task<TReturn>> map)
        {
            return _hasValue
                ? Maybe.Some(await map(_value).ConfigureAwait(false))
                : Maybe.None<TReturn>();
        }

        public Maybe<TReturn> FlatMap<TReturn>(Func<TValue, Maybe<TReturn>> map)
        {
            return _hasValue
                ? map(_value)
                : Maybe.None<TReturn>();
        }

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

        public override int GetHashCode()
        {
            return _value != null
                ? _value.GetHashCode()
                : 0;
        }

        // diagnostics

        public override string ToString()
        {
            var typeName = typeof(TValue).Name;

            return _hasValue
                ? $"Some<{typeName}>({_value})"
                : $"None<{typeName}>()";
        }
    }
}
