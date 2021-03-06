﻿namespace CodeGolf.Domain.Repositories
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Optional;

    public interface IHoleRepository
    {
        Task<Option<HoleInstance>> GetCurrentHole(CancellationToken cancellationToken);

        Task EndHole(Guid holeId, DateTime closeTime);

        Task AddHole(HoleInstance hole);

        Task ClearAll();
    }
}
