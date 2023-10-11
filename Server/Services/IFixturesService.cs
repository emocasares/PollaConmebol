using PollaEngendrilClientHosted.Shared.Models.ViewModel;

namespace PollaEngendrilClientHosted.Server.Services
{
    public interface IFixturesService
    {
        List<FixtureViewModel> GetFixture();
    }
}
