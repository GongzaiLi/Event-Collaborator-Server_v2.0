using System;
using System.Linq;
using System.Threading.Tasks;
using event_client_app.Models;
using event_client_app.Services.IRepository;
using Microsoft.EntityFrameworkCore;

namespace event_client_app.Services.Repository
{
    public class UserRepository : IUserRepository, IDisposable
    {
        private DBAPPContext _dbContext;

        public UserRepository(DBAPPContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void InsertUser(User user)
        {
            _dbContext.User.Add(user);
        }

        public async void Save()
        {
            _dbContext.SaveChanges();
        }

        public Task<User> FindUserByEmail(string email)
        {
            return _dbContext.User.SingleOrDefaultAsync(u => u.Email == email);
        }

        public void UpdateToken(User user, string token)
        {
            user.AuthToken = token;
        }

        public Task<User> FindUserByToken(string token)
        {
            return _dbContext.User.SingleOrDefaultAsync(u => u.AuthToken == token);
        }

        public User FindUserById(int id)
        {
            return _dbContext.User.Find(id);
        }

        public void DeleteToken(User user)
        {
            user.AuthToken = null;
        }

        public bool CheckEmailNotUseInOtherUsers(int id, string email)
        {
            var users = _dbContext.User.Where(u => u.Email == email && u.UserId != id).ToList();
            return users.Count == 0;
        }

        public void Dispose()
        {
            // _dbContext?.Dispose(); // ????
        }
    }
}