namespace PollaEngendrilClientHosted.Server.Services
{
    public interface IUsersService
    {
        int GetUserIdByUsernameOrNickname(string username, string nickname);
        int CreateUser(string username, string nickname);
    }
}
