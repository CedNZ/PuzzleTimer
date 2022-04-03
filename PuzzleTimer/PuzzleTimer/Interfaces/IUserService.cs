using PuzzleTimer.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PuzzleTimer.Interfaces
{
    public interface IUserService
    {
        public Task<User> GetUser(int id);

        public Task<User> CreateUser(string name);

        public Task<IEnumerable<User>> FindUsersByName(string name);
    }
}
