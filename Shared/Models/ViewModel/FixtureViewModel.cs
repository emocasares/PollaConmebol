namespace PollaEngendrilClientHosted.Shared.Models.ViewModel;

public class FixtureViewModel
{
    public int Id { get; set; }
    public string? Date { get; set; }
    public string? Time { get; set; }
    public string? HomeTeam { get; set; }
    public string? HomeTeamFlag { get; set; }
    public int? HomeTeamPredictedScore { get; set; }
    public int? HomeTeamRealScore { get; set; }
    public string? AwayTeam { get; set; }
    public string? AwayTeamFlag { get; set; }
    public int? AwayTeamPredictedScore { get; set; }
    public int? AwayTeamRealScore { get; set; }
    public string? User { get; set; }
    public bool IsLocked { get; set; } = false;
    public bool IsHomeScoreInvalid { get; set; } = false;
    public bool IsAwayScoreInvalid { get; set; } = false;
}
