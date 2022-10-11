using Microsoft.EntityFrameworkCore;
using ReferMeAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReferMeAPI
{
    public class ReferMeDBContext : DbContext
    {
        public ReferMeDBContext(DbContextOptions<ReferMeDBContext>options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<College> Colleges { get; set; }
        public DbSet<Company> Companies { get; set; }
    }
}
