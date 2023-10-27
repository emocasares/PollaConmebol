namespace PollaEngendrilClientHosted.Client.Services
{
    public interface IUsersApiService
    {
        Task<int> GetUserIdByUserNameOrNickname(string username, string nickname);

        Task<int> CreateUser(string username, string nickname);
    }
}
