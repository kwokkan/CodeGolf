namespace CodeGolf.Persistence.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using CodeGolf.Domain;
    using CodeGolf.Domain.Repositories;
    using Microsoft.EntityFrameworkCore;
    using Optional;

    public class HoleRepository : IHoleRepository
    {
        private readonly CodeGolfContext context;

        public HoleRepository(CodeGolfContext context)
        {
            this.context = context;
        }

        async Task<Option<Hole>> IHoleRepository.GetCurrentHole(CancellationToken cancellationToken)
        {
            if (await this.context.Holes.AnyAsync(cancellationToken))
            {
                return Option.Some(await this.context.Holes.LastAsync(cancellationToken));
            }
            else
            {
                return Option.None<Hole>();
            }
        }

        async Task IHoleRepository.EndHole(Guid holeId, DateTime closeTime)
        {
            var hole = await this.context.Holes.SingleAsync(a => a.HoleId == holeId);
            hole.SetEnd(closeTime);
            this.context.Update(hole);
            await this.context.SaveChangesAsync();
        }

        Task IHoleRepository.AddHole(Hole hole)
        {
            this.context.Holes.Add(hole);
            return this.context.SaveChangesAsync();
        }

        Task IHoleRepository.ClearAll()
        {
            this.context.Holes.RemoveRange(this.context.Holes);
            return this.context.SaveChangesAsync();
        }

        Task IHoleRepository.Update(Hole hole)
        {
            this.context.Attach(hole);
            this.context.Entry(hole).State = EntityState.Modified;
            this.context.Update(hole);
            return this.context.SaveChangesAsync();
        }

        Task<IReadOnlyList<Hole>> IHoleRepository.GetGameHoles(Guid gameId)
        {
            return this.context.Holes.Where(a => a.GameId == gameId).ToReadOnlyAsync(CancellationToken.None);
        }
    }
}
