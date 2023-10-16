using PollaEngendrilClientHosted.Shared.Models.Entity;

namespace PollaEngendrilClientHosted.Server.Services.UserEligibleSpecification
{
    public class UserEligibleSpecificationAlwaysTrue : IUserEligibleSpecification
    {
        public bool IsSatisfiedBy(User user)
        {
            return true;
        }

    }
}