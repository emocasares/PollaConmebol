using PollaEngendrilClientHosted.Shared.Models.Entity;

namespace PollaEngendrilClientHosted.Server.Services.UserEligibleSpecification
{
    public interface IUserEligibleSpecification
    {
        bool IsSatisfiedBy(User user);
    }
}
