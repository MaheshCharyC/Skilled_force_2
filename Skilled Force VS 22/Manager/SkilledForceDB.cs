using Microsoft.EntityFrameworkCore;
using Skilled_Force_VS_22.Models.DB;

namespace Skilled_Force_VS_22.Manager
{
    public class SkilledForceDB : DbContext
    {

        public DbSet<User> User { get; set; }

        public DbSet<Job> Job { get; set; }

        public DbSet<Role> Role { get; set; }

        public DbSet<Company> Company { get; set; }

        public DbSet<CompanyReview> CompanyReview { get; set; }

        public SkilledForceDB(DbContextOptions<SkilledForceDB> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>()
                        .HasMany(e => e.CompanyReviews)
                        .WithOne(e => e.User)
                        .HasForeignKey(e => e.UserId)
                        .OnDelete(DeleteBehavior.NoAction);

            //modelBuilder.Entity<Job>()
            //.HasOne(e => e.Company)
            //.WithOne(e => e.User)
            //.HasForeignKey(e => e.UserId)
            //.OnDelete(DeleteBehavior.NoAction);

        }
    }
}
