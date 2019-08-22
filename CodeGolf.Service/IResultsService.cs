namespace CodeGolf.Service
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using CodeGolf.Service.Dtos;

    public interface IResultsService
    {
        Task<IReadOnlyList<ResultDto>> GetFinalScores(Guid gameId,  CancellationToken cancellationToken);
    }
}
