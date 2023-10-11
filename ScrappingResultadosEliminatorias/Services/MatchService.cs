using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using PollaEngendrilClientHosted.Shared.Models.Entity;
using PollaEngendrilClientHosted.Server.Data;

namespace ScrappingResultadosEliminatorias.Services;
public class MatchService
{
    private readonly ApplicationDbContext _context;

    public MatchService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task InsertOrUpdateMatchAsync(Match match)
    {
        var existingMatch = await _context.Matches
            .FirstOrDefaultAsync(m => m.HomeTeam == match.HomeTeam && m.AwayTeam == match.AwayTeam);

        if (existingMatch == null)
        {
            _context.Matches.Add(match);
        }
        else
        {
            existingMatch.HomeTeamFlag = match.HomeTeamFlag;
            existingMatch.AwayTeamFlag = match.AwayTeamFlag;
            existingMatch.HomeTeamScore = match.HomeTeamScore;
            existingMatch.AwayTeamScore = match.AwayTeamScore;
            existingMatch.Date = match.Date;
        }

        await _context.SaveChangesAsync();
    }
}
