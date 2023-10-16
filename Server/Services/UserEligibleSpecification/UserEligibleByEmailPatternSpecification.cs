using PollaEngendrilClientHosted.Shared.Models.Entity;

namespace PollaEngendrilClientHosted.Server.Services.UserEligibleSpecification
{
    public class UserEligibleByEmailPatternSpecification : IUserEligibleSpecification
    {
        public bool IsSatisfiedBy(User user)
        {
            return !user.UserName.Contains("@mailinator.com");
        }
    }
}
