# SimpleMaybe

_This library is under development!_

This is a C# implementation of maybe/optional type.

It was created because I needed maybe implementation which:

1. guarded against nulls (you cannot create `Maybe.Some<string>(null)`),
1. had support for async.

It has no documentation at the moment, but you can check tests project to see what it can do.

Library was highly inspired by projects like [nlkl/Optional](https://github.com/nlkl/Optional) or [zoran-horvat/option](https://github.com/zoran-horvat/option).