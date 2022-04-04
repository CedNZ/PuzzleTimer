using System.Collections.Generic;
using System.Threading.Tasks;
using PuzzleTimer.Models;

namespace PuzzleTimer.Interfaces
{
    public interface IUserRepository
    {
        public Task<User> GetUser(int id);

        public Task<User> CreateUser(string name);

        public Task<IEnumerable<User>> FindUsersByName(string name);

        public Task<IEnumerable<User>> GetUsersForSession(int sessionId);
    }
}
