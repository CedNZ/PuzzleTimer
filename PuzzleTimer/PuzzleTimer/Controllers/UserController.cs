using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PuzzleTimer.Interfaces;
using PuzzleTimer.Models;

namespace PuzzleTimer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet(nameof(CreateUser), Name = nameof(CreateUser))]
        public async Task<User> CreateUser([FromQuery] string userName)
        {
            return await _userService.CreateUser(userName);
        }

        [HttpGet(nameof(FindUsersByName), Name = nameof(FindUsersByName))]
        public async Task<IEnumerable<User>> FindUsersByName([FromQuery] string name)
        {
            return await _userService.FindUsersByName(name);
        }
    }
}
