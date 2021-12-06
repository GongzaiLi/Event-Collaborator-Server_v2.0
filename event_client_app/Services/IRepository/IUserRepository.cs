using System;
using System.Threading.Tasks;
using event_client_app.Models;

namespace event_client_app.Services.IRepository
{
    public interface IUserRepository : IDisposable
    {
        void InsertUser(User user);
        void Save();
        Task<User> FindUserByEmail(string email);
        void UpdateToken(User user, string token);
        Task<User> FindUserByToken(string token);
        
        User FindUserById(int id);
        void DeleteToken(User user);
        
        bool CheckEmailNotUseInOtherUsers(int id, string email);
        
    }
}