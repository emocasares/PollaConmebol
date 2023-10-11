using PollaEngendrilClientHosted.Shared.Models.ViewModel;

namespace PollaEngendrilClientHosted.Client.Services;

public interface IFixturesApiService
{
    Task<IEnumerable<FixtureViewModel>> GetFixtures();
}