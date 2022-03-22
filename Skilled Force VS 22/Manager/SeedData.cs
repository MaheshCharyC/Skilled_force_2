using Microsoft.EntityFrameworkCore;
using Skilled_Force_VS_22.Models;
using System.Linq;

namespace Skilled_Force_VS_22.Manager
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new SkilledForceDB(
                serviceProvider.GetRequiredService<
                    DbContextOptions<SkilledForceDB>>()))
            {
                if (context.User.Any())
                {
                    return;   // DB has been seeded
                }

                context.Role.AddRange(
                    new Role()
                    {   
                        RoleId = "1",
                        Name = "Job Seeker",
                        Description = "General user/job seeker"
                    }, 
                    new Role()
                    {
                        RoleId = "2",
                        Name = "Recruiter",
                        Description = "General user/job provider"
                    },
                    new Role()
                    {
                        RoleId = "3",
                        Name = "Admin",
                        Description = "Admin user"
                    }
                );

                context.User.AddRange(
                    new User
                    {
                        Email ="seeker@gmail.com",
                        Password="test",
                        FirstName="John",
                        LastName= "Smith",
                        Gender="Male",
                        RoleId = "1",
                        Phone ="4989895454"
                    },
                    new User
                    {
                        Email = "recruiter@gmail.com",
                        Password = "test",
                        FirstName = "Lisa",
                        LastName = "phill",
                        Gender = "Female",
                        RoleId = "2",
                        Phone = "5989895454"
                    },
                    new User
                    {
                        Email = "admin@gmail.com",
                        Password = "test",
                        FirstName = "Admin F",
                        LastName = "Admin L",
                        Gender = "Male",
                        RoleId = "3",
                        Phone = "6989895454"
                    }

                );
                context.Job.AddRange(
                    new Job
                    {
                        Title = "Test Job 1",
                        Description = "Test data 2",
                        JobType= "Front End Developer",
                        EmploymentType="Full Time",
                        Salary="5000$ - 10000$ PM",
                        Location="New York",
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        CreatedBy = "2",
                        UpdatedBy = "2",
                        IsApplied=false
                    }, new Job
                    {
                        Title = "Test Job 2",
                        Description = "Test data 2",
                        JobType = "Java Developer",
                        EmploymentType = "Part Time",
                        Salary = "10000$ - 15000$ PM",
                        Location = "Chicago",
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        CreatedBy = "2",
                        UpdatedBy = "2",
                        IsApplied = false
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
