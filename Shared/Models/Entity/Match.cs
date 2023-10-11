namespace PollaEngendrilClientHosted.Shared.Models.Entity;
public class Match
{
    public int Id { get; set; }
    public string HomeTeam { get; set; }
    public string AwayTeam { get; set; }
    public string HomeTeamFlag { get; set; }
    public string AwayTeamFlag { get; set; }
    public int HomeTeamScore { get; set; }
    public int AwayTeamScore { get; set; }
    public DateTime Date { get; set; }
}
