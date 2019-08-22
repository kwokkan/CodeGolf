namespace CodeGolf.Domain.Repositories
{
    using System;
    using System.Collections.Generic;
    using Optional;

    public interface IGameRepository
    {
        IReadOnlyList<Game> GetMyGames(int userId);

        Game GetGame(Guid gameId);

        Option<Hole> GetByHoleId(Guid holeId);

        Option<Hole> GetAfter(Guid holeId);
    }
}
