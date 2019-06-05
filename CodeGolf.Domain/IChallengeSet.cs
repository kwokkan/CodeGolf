﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Optional;

namespace CodeGolf.Domain
{
    public interface IChallengeSet
    {
        Task<IReadOnlyList<ChallengeResult>> GetResults(Func<IChallenge, Task<Option<object, string>>> t);

        IReadOnlyList<Type> Params { get; }

        Type ReturnType { get; }

        string Title { get; }

        string Description { get; }

        IReadOnlyList<IChallenge> Challenges { get; }
    }
}