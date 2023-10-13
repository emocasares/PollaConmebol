using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PollaEngendrilClientHosted.Shared.Models.ViewModel
{
    public class OtherUserPredictionViewModel
    {
        public int MatchId { get; set; }
        public string UserName { get; set; }
        public int HomeTeamPredictedScore { get; set; }
        public int AwayTeamPredictedScore { get; set; }
        public int PointsObtained {  get; set; }
    }
}
