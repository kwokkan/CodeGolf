﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeGolf.Domain.ChallengeInterfaces;
using EnsureThat;
using Optional;

namespace CodeGolf.Domain
{
    public class ChallengeSet<T> : IChallengeSet
    {
        public ChallengeSet(string title, string description, IReadOnlyList<Type> ps,
            IReadOnlyList<Challenge<T>> challenges)
        {
            this.Title = EnsureArg.IsNotNull(title, nameof(title));
            this.Description = EnsureArg.IsNotNull(description, nameof(description));
            this.Params = EnsureArg.IsNotNull(ps, nameof(ps));
            this.Challenges = EnsureArg.IsNotNull(challenges, nameof(challenges));
            foreach (var challenge in challenges)
            {
                this.ValidateParameters(challenge);
            }
        }

        private static bool IsMisMatched(ValueTuple<object, Type> t) => t.Item1.GetType() != t.Item2;

        private void ValidateParameters(IChallenge challenge)
        {
            if (challenge.Args.Length != this.Params.Count)
            {
                throw new Exception("Incorrect number of parameters");
            }

            var misMatched = challenge.Args.Zip(this.Params, ValueTuple.Create).Where(IsMisMatched);
            if (misMatched.Any())
            {
                throw new Exception("Mismatched parameters");
            }
        }

        public string Title { get; }

        public string Description { get; }

        public IReadOnlyList<Type> Params { get; }

        Type IChallengeSet.ReturnType => typeof(T);

        private IReadOnlyList<Challenge<T>> Challenges { get; }

        IReadOnlyList<IChallenge> IChallengeSet.Challenges => this.Challenges;

        async Task<IReadOnlyList<ChallengeResult>> IChallengeSet.GetResults(
            CompileResult t)
        {
            var reses = await t.Func(this.Challenges.Select(a => a.Args).ToArray());
            var challRes = reses.Zip(this.Challenges, Tuple.Create);

            return challRes.Select(challenge =>
            {
                var errors = challenge.Item1.Match(success =>
                {
                    var res = success;
                    if (!AreEqual(challenge.Item2.ExpectedResult, res))
                    {
                        return Option.Some(
                            $"Return value incorrect. Expected: {GenericPresentationHelpers.WrapIfArray(challenge.Item2.ExpectedResult, typeof(T))}, Found: {GenericPresentationHelpers.WrapIfArray(res, typeof(T))}");
                    }

                    return Option.None<string>();
                }, Option.Some);
                return new ChallengeResult(errors, challenge.Item2);
            }).ToList();
        }

        private static bool AreEqual(object expect, object actual)
        {
            return expect.Equals(actual);
        }
    }
}
