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
                        FirstName="JobSeekeer F",
                        LastName= "L",
                        Gender="Male",
                        RoleId = "1",
                        Phone ="0000000000"
                    },
                    new User
                    {
                        Email = "recruiter@gmail.com",
                        Password = "test",
                        FirstName = "Recruiter F",
                        LastName = "Recruiter L",
                        Gender = "Female",
                        RoleId = "2",
                        Phone = "0000000000"
                    },
                    new User
                    {
                        Email = "admin@gmail.com",
                        Password = "test",
                        FirstName = "Admin F",
                        LastName = "Admin L",
                        Gender = "Male",
                        RoleId = "3",
                        Phone = "0000000000"
                    }

                );
                context.Job.AddRange(
                    new Job
                    {
                        Title = "Test Job 1",
                        Description = "Test data 2",
                        JobType="",
                        EmploymentType="",
                        Salary="",
                        Location="",
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        CreatedBy = "2",
                        UpdatedBy = "2",
                        IsApplied=false
                    }, new Job
                    {
                        Title = "Test Job 2",
                        Description = "Test data 2",
                        JobType = "",
                        EmploymentType = "",
                        Salary = "",
                        Location = "",
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
