# SimpleMaybe

[![AppVeyor](https://img.shields.io/appveyor/ci/redss/simplemaybe.svg)](https://ci.appveyor.com/project/redss/simplemaybe)
[![NuGet](https://img.shields.io/nuget/v/SimpleMaybe.svg)](https://www.nuget.org/packages/SimpleMaybe)

_This library is under development!_

This is a C# implementation of maybe/optional type.

Installation:

```
PM> Install-Package SimpleMaybe
```

It was created because I needed maybe implementation which:

1. guarded against nulls (you cannot create `Maybe.Some<string>(null)`),
1. had support for async.

It has no documentation at the moment, but you can check tests project to see what it can do.

Library was highly inspired by projects like [nlkl/Optional](https://github.com/nlkl/Optional) or [zoran-horvat/option](https://github.com/zoran-horvat/option).