using Microsoft.EntityFrameworkCore;
using Skilled_Force_VS_22.Models;
using Skilled_Force_VS_22.Models.DB;
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
                        Email = "seeker@gmail.com",
                        Password = "test",
                        FirstName = "John",
                        LastName = "Smith",
                        Gender = "Male",
                        RoleId = "1",
                        CompanyId = "1",
                        Phone = "4989895454"
                    },
                    new User
                    {
                        Email = "recruiter@gmail.com",
                        Password = "test",
                        FirstName = "Lisa",
                        LastName = "phill",
                        Gender = "Female",
                        RoleId = "2",
                        CompanyId = "1",
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
                        CompanyId = "1",
                        Phone = "6989895454",
                        UserId = "3",
                    }

                );
                context.Job.AddRange(
                    new Job
                    {
                        Title = "Test Job 1",
                        Description = "Test data 2",
                        JobType = "Front End Developer",
                        EmploymentType = "Full Time",
                        Salary = "5000$ - 10000$ PM",
                        Location = "New York",
                        CompanyId = "1",
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        CreatedBy = "2",
                        UpdatedBy = "2",
                        IsApplied = false
                    }, new Job
                    {
                        Title = "Test Job 2",
                        Description = "Test data 2",
                        JobType = "Java Developer",
                        EmploymentType = "Part Time",
                        Salary = "10000$ - 15000$ PM",
                        Location = "Chicago",
                        CompanyId = "1",
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        CreatedBy = "2",
                        UpdatedBy = "2",
                        IsApplied = false
                    }
                );

                context.Company.Add(new Company
                {
                    CompanyId = "1",
                    Name = "NumberOne",
                    Description = "This is the Number one company. This is just an example which shows that a company can have some description",
                    UserId = "3"
                });

                context.CompanyReview.Add(new CompanyReview
                {
                    CompanyId = "1",
                    Rating = 5,
                    UserId = "3",
                    comment = "I Give 5",
                    Time = DateTime.Now
                });

                context.CompanyReview.Add(new CompanyReview
                {
                    CompanyId = "1",
                    Rating = 1,
                    UserId = "3",
                    comment = "I Give 1",
                    Time = DateTime.Now
                });

                context.CompanyReview.Add(new CompanyReview
                {
                    CompanyId = "1",
                    Rating = 3,
                    UserId = "3",
                    comment = "I Give 3",
                    Time = DateTime.Now
                });

                context.SaveChanges();
            }
        }
    }
}
