namespace PollaEngendrilClientHosted.Client.Services
{
    public interface IUsersApiService
    {
        Task<int> GetUserIdByUserName(string username);
        Task<int> CreateUser(string username);
    }
}
