using Microsoft.EntityFrameworkCore;
using Skilled_Force_VS_22.Models;
using System.Linq;

namespace Skilled_Force_VS_22.Manager
{
    public class SkilledForceDB : DbContext
    {

        public DbSet<User> User { get; set; }

        public DbSet<Job> Job { get; set; }

        public DbSet<Role> Role { get; set; }

        public SkilledForceDB(DbContextOptions<SkilledForceDB> options)
            : base(options)
        {

        }
    }
}
