﻿using System.Threading.Tasks;
using CodeGolf.Service;
using CodeGolf.Service.Dtos;
using CodeGolf.Web.Tooling;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CodeGolf.Web.Pages
{
    public class GameModel : PageModel
    {
        private readonly IGameService gameService;
        private readonly IIdentityTools identityTools;

        [BindProperty]
        public string Code { get; set; }

        public int? Score { get; private set; }

        public ErrorSet Errors { get; private set; } = new ErrorSet();

        public ChallengeSet<string> ChallengeSet { get; }

        public GameModel(IGameService gameService, IIdentityTools identityTools)
        {
            this.gameService = gameService;
            this.identityTools = identityTools;
            this.ChallengeSet = gameService.GetCurrent().ChallengeSet;
        }

        public void OnGet()
        {

        }

        public async Task OnPost()
        {
            var res = await this.gameService.Attempt(this.identityTools.GetIdentity(this.Request), this.Code, this.ChallengeSet).ConfigureAwait(false);
            res.Match(a => this.Score = a, err => this.Errors = err);
        }
    }
}