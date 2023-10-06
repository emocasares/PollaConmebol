namespace PollaEngendrilClientHosted.Shared
{
    public class Match
    {
        public int MatchId { get; set; }
        public DateTime Date { get; set; }
        public int HomeTeamId { get; set; }
        public int AwayTeamId { get; set; }
        public int ScoreHomeTeam{ get; set; }
        public int ScoreAwayTeam { get; set; }
    }
}
