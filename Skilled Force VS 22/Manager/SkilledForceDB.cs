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

        public DbSet<Chat> Chat { get; set; }

        public DbSet<Message> Message { get; set; }

        public DbSet<JobApplication> JobApplication { get; set; }

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

            modelBuilder.Entity<User>()
                        .HasMany(e => e.CreatedJobs)
                        .WithOne(e => e.CreatedBy)
                        .HasForeignKey(e => e.CreatedByUserId)
                        .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<User>()
                        .HasMany(e => e.SentChats)
                        .WithOne(e => e.FromUser)
                        .HasForeignKey(e => e.FromUserId)
                        .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<User>()
                        .HasMany(e => e.ReceivedChats)
                        .WithOne(e => e.ToUser)
                        .HasForeignKey(e => e.ToUserId)
                        .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<User>()
                        .HasMany(e => e.SentMessages)
                        .WithOne(e => e.FromUser)
                        .HasForeignKey(e => e.FromUserId)
                        .OnDelete(DeleteBehavior.NoAction);

            //     modelBuilder.Entity<Job>()
            //.HasOne(j => j.CreatedBy)
            //.WithMany(u => u.CreatedJobs)
            //.HasForeignKey<Job>(m => m.UserId1)
            //.OnDelete(DeleteBehavior.Restrict);


            //modelBuilder.Entity<Job>().HasOne(e => e.CreatedBy)
            //    .WithOne()
            //    .HasForeignKey<Job>(e => e.CreatedByUserId);



            //modelBuilder.Entity<Job>()
            //.HasOne(e => e.Company)
            //.WithOne(e => e.User)
            //.HasForeignKey(e => e.UserId)
            //.OnDelete(DeleteBehavior.NoAction);

        }
    }
}
