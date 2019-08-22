namespace CodeGolf.Domain.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Optional;

    public interface IHoleRepository
    {
        Task<Option<Hole>> GetCurrentHole(CancellationToken cancellationToken);

        Task EndHole(Guid holeId, DateTime closeTime);

        Task AddHole(Hole hole);

        Task ClearAll();

        Task Update(Hole hole);

        Task<IReadOnlyList<Hole>> GetGameHoles(Guid gameId);
    }
}
