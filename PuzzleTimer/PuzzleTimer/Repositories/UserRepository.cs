using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PuzzleTimer.Interfaces;
using PuzzleTimer.Models;

namespace PuzzleTimer.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbContextFactory<ApplicationContext> _contextFactory;

        public UserRepository(IDbContextFactory<ApplicationContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<User> CreateUser(string name)
        {
            using (var ctx = _contextFactory.CreateDbContext())
            {
                User user = new User { Name = name };
                ctx.Users.Add(user);
                await ctx.SaveChangesAsync();
                return user;
            }
        }

        public async Task<IEnumerable<User>> FindUsersByName(string name)
        {
            using (var ctx = _contextFactory.CreateDbContext())
            {
                return await ctx.Users.Where(u => EF.Functions.Like(u.Name, $"{name}%")).ToListAsync();
            }
        }

        public async Task<User> GetUser(int id)
        {
            using (var ctx = _contextFactory.CreateDbContext())
            {
                return await ctx.Users.FindAsync(id);
            }
        }

        public async Task<IEnumerable<User>> GetUsersForSession(int sessionId)
        {
            using (var ctx = _contextFactory.CreateDbContext())
            {
                var session = await ctx.SolvingSessions
                    .Include(s => s.Users)
                    .FirstOrDefaultAsync(s => s.Id == sessionId);

                return session.Users.ToList();
            }
        }
    }
}
