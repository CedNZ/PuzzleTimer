using PuzzleTimer.Interfaces;
using PuzzleTimer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleTimer.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> CreateUser(string name)
        {
            return await _userRepository.CreateUser(name);
        }

        public async Task<IEnumerable<User>> FindUsersByName(string name)
        {
            return await _userRepository.FindUsersByName(name);
        }

        public async Task<User> GetUser(int id)
        {
            return await _userRepository.GetUser(id);
        }
    }
}
