namespace PollaEngendrilClientHosted.Server.Services
{
    public interface IUsersService
    {
        int GetUserIdByUsername(string username);
        int CreateUser(string username);
    }
}
