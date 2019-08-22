namespace CodeGolf.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using CodeGolf.Domain.ChallengeInterfaces;
    using CodeGolf.Service;
    using CodeGolf.Service.Dtos;
    using CodeGolf.Web.Attributes;
    using CodeGolf.Web.Mappers;
    using CodeGolf.Web.Tooling;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Optional.Unsafe;

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [ServiceFilter(typeof(GameAdminAuthAttribute))]
    public class AdminController : ControllerBase
    {
        private readonly IDashboardService dashboardService;
        private readonly IResultsService resultsService;
        private readonly IAdminService adminService;
        private readonly ChallengeSetMapper challengeSetMapper;
        private readonly IIdentityTools identityTools;

        public AdminController(IDashboardService dashboardService, ChallengeSetMapper challengeSetMapper, IResultsService resultsService, IIdentityTools identityTools, IAdminService adminService)
        {
            this.dashboardService = dashboardService;
            this.challengeSetMapper = challengeSetMapper;
            this.resultsService = resultsService;
            this.adminService = adminService;
            this.identityTools = identityTools;
        }

        [HttpPost("[action]")]
        public async Task EndHole(CancellationToken cancellationToken)
        {
            var curr = await this.dashboardService.GetCurrentHole(cancellationToken);
            await curr.Match(
                some => this.dashboardService.EndHole(some.Hole.HoleId),
                () => Task.CompletedTask);
        }

        [HttpPost("[action]")]
        public Task NextHole(Guid gameId, CancellationToken cancellationToken)
        {
            return this.dashboardService.NextHole(gameId, cancellationToken);
        }

        [HttpGet("[action]/{holeId}")]
        public async Task<ActionResult<IReadOnlyList<AttemptDto>>> Results(Guid holeId, CancellationToken cancellationToken)
        {
            return (await this.dashboardService.GetAttempts(holeId, cancellationToken)).Match(some => new ActionResult<IReadOnlyList<AttemptDto>>(some), () => this.NotFound());
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<Models.HoleDto>> CurrentHole(CancellationToken cancellationToken)
        {
            return (await this.dashboardService.GetCurrentHole(cancellationToken)).Match<ActionResult<Models.HoleDto>>(
                some => new Models.HoleDto(some.Hole, some.Start, some.End, some.ClosedAt, some.HasNext, this.challengeSetMapper.Map(some.ChallengeSet)),
                () => null);
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<IReadOnlyList<ResultDto>>> FinalScores(Guid gameId, CancellationToken cancellationToken)
        {
            var r = await this.resultsService.GetFinalScores(gameId, cancellationToken);
            return new ActionResult<IReadOnlyList<ResultDto>>(r);
        }

        [HttpGet("[action]/{attemptId}")]
        public async Task<ActionResult<AttemptCodeDto>> Attempt(Guid attemptId, CancellationToken cancellationToken)
        {
            var res = await this.dashboardService.GetAttemptById(attemptId, cancellationToken);
            return res.Match(
                some => new ActionResult<AttemptCodeDto>(some),
                () => this.NotFound());
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<IReadOnlyList<GameDto>>> MyGames(CancellationToken cancellationToken)
        {
            var user = this.identityTools.GetIdentity(this.HttpContext).ValueOrFailure();
            var r = await this.adminService.GetAllGames(user, cancellationToken);
            return new ActionResult<IReadOnlyList<GameDto>>(r);
        }

        [HttpPost("[action]")]
        public Task CreateGame(GameDto challenges, string accessKey, CancellationToken cancellationToken)
        {
            var user = this.identityTools.GetIdentity(this.HttpContext).ValueOrFailure();
            return this.adminService.CreateGame(challenges, accessKey, user, cancellationToken);
        }

        [HttpGet("[action]")]
        public Task<IReadOnlyList<IChallengeSet>> AllChallenges(CancellationToken cancellationToken)
        {
            return this.adminService.GetAllChallenges(cancellationToken);
        }
    }
}
