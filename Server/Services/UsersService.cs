using PollaEngendrilClientHosted.Server.Data;

namespace PollaEngendrilClientHosted.Server.Services
{
    public class UsersService : IUsersService
    {
        private readonly ApplicationDbContext dbContext;
        public UsersService(ApplicationDbContext context)
        {
            this.dbContext = context;
        }

        public int GetUserIdByUsernameOrNickname(string username, string nickname)
        {
            var user = dbContext.Users.FirstOrDefault(u => u.Name == username);

            if (user != null)
            {
                user.NickName = nickname;
                dbContext.SaveChanges();
                return user.Id;
            }
            else
            {
                user = dbContext.Users.FirstOrDefault(u => u.NickName == nickname);
                if (user != null)
                {
                    return user.Id;
                }

                return -1;
            }
        }

        public int CreateUser(string username, string nickname)
        {
            var user = dbContext.Users.FirstOrDefault(u => u.NickName == nickname || u.Name == username);
            if (user == null)
            {
                dbContext.Users.Add(new Shared.Models.Entity.User() { Name = username, NickName = nickname });
                dbContext.SaveChanges();
                user = dbContext.Users.FirstOrDefault(u => u.NickName == nickname);
            }
            else
            {
                user.Name = username;
                user.NickName = nickname;
                dbContext.SaveChanges();
                user = dbContext.Users.FirstOrDefault(u => u.NickName == nickname);
            }
            return user.Id;
        }
    }
}
