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

        public int GetUserIdByUsername(string username)
        {
            var user = dbContext.Users.FirstOrDefault(u => u.UserName == username);

            if (user != null)
            {
                return user.Id;
            }
            else
            {
                return -1;
            }
        }

        public int CreateUser(string username)
        {
            var user = dbContext.Users.FirstOrDefault(u => u.UserName == username);
            if (user == null)
            {
                dbContext.Users.Add(new Shared.Models.Entity.User() { UserName = username });
                dbContext.SaveChanges();
                user = dbContext.Users.FirstOrDefault(u => u.UserName == username);
            }
            return user.Id;
        }
    }
}
