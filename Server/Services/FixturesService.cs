﻿using PollaEngendrilClientHosted.Server.Data;
using PollaEngendrilClientHosted.Shared.Models.ViewModel;

namespace PollaEngendrilClientHosted.Server.Services
{
    public class FixturesService : IFixturesService
    {
        private readonly ApplicationDbContext dbContext;
        public FixturesService(ApplicationDbContext context)
        {
            this.dbContext = context;
        }
        public List<FixtureViewModel> GetFixture(string username)
        {
            var matches = dbContext.Matches.ToList();
            var user = dbContext.Users.FirstOrDefault(u => u.UserName == username);
            var predictions = dbContext.Predictions.Where(u => u.UserId == user.Id).ToList();
            var fixtures = matches.Select(match =>
            {
                var prediction = user != null ? predictions.FirstOrDefault(p => p.MatchId == match.Id) : null;
                return new FixtureViewModel
                {
                    Id = match.Id,
                    Date = match.Date.ToString("yyyy-MM-dd"),
                    HomeTeam = match.HomeTeam,
                    HomeTeamFlag = match.HomeTeamFlag,
                    HomeTeamPredictedScore = prediction?.HomeTeamScore,
                    HomeTeamRealScore = match.HomeTeamScore,
                    AwayTeam = match.AwayTeam,
                    AwayTeamFlag = match.AwayTeamFlag,
                    AwayTeamPredictedScore = prediction?.AwayTeamScore,
                    AwayTeamRealScore = match.AwayTeamScore,
                    User = username
                };
            }).ToList();
            return fixtures;
        }
    }
}
