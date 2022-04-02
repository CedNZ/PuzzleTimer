﻿using Microsoft.EntityFrameworkCore;
using PuzzleTimer.Interfaces;
using PuzzleTimer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                return await ctx.Users.Where(u => u.Name.StartsWith(name)).ToListAsync();
            }
        }

        public async Task<User> GetUser(int id)
        {
            using (var ctx = _contextFactory.CreateDbContext())
            {
                return await ctx.Users.FindAsync(id);
            }
        }
    }
}
