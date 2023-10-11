using PollaEngendrilClientHosted.Shared.Models.ViewModel;

namespace PollaEngendrilClientHosted.Server.Services
{
    public class FixturesService : IFixturesService
    {
        public List<FixtureViewModel> GetFixture()
        {
            var fixtures = new List<FixtureViewModel>();
            fixtures.Add( new FixtureViewModel() { Date = "10/12", Id=1, HomeTeam="Bolivia", HomeTeamFlag = "https://ssl.gstatic.com/onebox/media/sports/logos/SGxeD7yhwPj53FmPBmMMHg_48x48.png", AwayTeam= "Ecuador", AwayTeamFlag = "https://ssl.gstatic.com/onebox/media/sports/logos/AKqvkBpIyr-iLOK7Ig7-yQ_48x48.png" } );
            return fixtures;
        }
    }
}
