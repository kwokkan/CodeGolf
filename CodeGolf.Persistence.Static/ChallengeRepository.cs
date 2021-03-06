namespace CodeGolf.Persistence.Static
{
    using System;
    using System.Collections.Generic;

    using CodeGolf.Domain.ChallengeInterfaces;
    using CodeGolf.Domain.Repositories;

    using Optional;
    using Optional.Collections;

    public class ChallengeRepository : IChallengeRepository
    {
        private static readonly IReadOnlyList<IChallengeSet> Cs = new IChallengeSet[] { Challenges.HelloWorld, Challenges.AlienSpeak, Challenges.FizzBuzz, Challenges.Calculator };

        Option<IChallengeSet> IChallengeRepository.GetById(Guid id)
        {
            return Cs.SingleOrNone(a => a.Id == id);
        }

        IReadOnlyList<IChallengeSet> IChallengeRepository.GetAll()
        {
            return Cs;
        }
    }
}
