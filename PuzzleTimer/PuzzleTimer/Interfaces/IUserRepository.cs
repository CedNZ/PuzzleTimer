using PuzzleTimer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleTimer.Interfaces
{
    public interface IUserRepository
    {
        public Task<User> GetUser(int id);

        public Task<User> CreateUser(string name);

        public Task<IEnumerable<User>> FindUsersByName(string name);
    }
}
