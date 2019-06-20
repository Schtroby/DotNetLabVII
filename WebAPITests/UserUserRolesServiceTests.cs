using LabIV.DTO;
using LabIV.Models;
using LabIV.Services;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebAPITests
{
    class UserUserRolesServiceTests
    {

        [Test]
        public void GetByIdShouldReturnUserRole()
        {
            var options = new DbContextOptionsBuilder<TasksDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(GetByIdShouldReturnUserRole))
              .Options;

            using (var context = new TasksDbContext(options))
            {
                var userUserRolesService = new UserUserRolesService(null, context);

                User userToAdd = new User
                {
                    Email = "ana@yahoo.com",
                    LastName = "Marcus",
                    FirstName = "Ana",
                    Password = "1234567",
                    RegistrationDate = DateTime.Now,
                    UserUserRoles = new List<UserUserRole>()
                };
                context.Users.Add(userToAdd);

                UserRole addUserRole = new UserRole
                {
                    Name = "Newcomer",
                    Description = "A new guy..."
                };
                context.UserRoles.Add(addUserRole);
                context.SaveChanges();

                context.UserUserRoles.Add(new UserUserRole
                {
                    User = userToAdd,
                    UserRole = addUserRole,
                    StartTime = DateTime.Now,
                    EndTime = null
                });
                context.SaveChanges();

                var userUserRoleGet = userUserRolesService.GetById(1);
                Assert.IsNotNull(userUserRoleGet.FirstOrDefaultAsync(uurole => uurole.EndTime == null));
                
            }
        }

        [Test]
        public void CreateShouldAddAnUserUserRole()
        {
            var options = new DbContextOptionsBuilder<TasksDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(CreateShouldAddAnUserUserRole))
              .Options;

            using (var context = new TasksDbContext(options))
            {
                var userUserRolesService = new UserUserRolesService(null, context);

                User userToAdd = new User
                {
                    Email = "ana@yahoo.com",
                    LastName = "Marcus",
                    FirstName = "Ana",
                    Password = "1234567",
                    RegistrationDate = DateTime.Now,
                    UserUserRoles = new List<UserUserRole>()
                };
                context.Users.Add(userToAdd);

                UserRole addUserRole = new UserRole
                {
                    Name = "Newcomer",
                    Description = "A new guy..."
                };
                context.UserRoles.Add(addUserRole);
                context.SaveChanges();

                context.UserUserRoles.Add(new UserUserRole
                {
                    User = userToAdd,
                    UserRole = addUserRole,
                    StartTime = DateTime.Now,
                    EndTime = null
                });
                context.SaveChanges();

               // var newUUR = new UserUserRolePostDTO
               // {
               //     UserId = userToAdd.Id,
               //     UserRoleName = "Admin"
               // };

               // var result = userUserRolesService.Create(newUUR);

                Assert.NotNull(userToAdd);
                Assert.AreEqual("Newcomer", addUserRole.Name);
                Assert.AreEqual("Ana", userToAdd.FirstName);
            }
        }


        [Test]
        public void GetUserRoleNameByIdShouldReturnUserRoleName()
        {
            var options = new DbContextOptionsBuilder<TasksDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(GetUserRoleNameByIdShouldReturnUserRoleName))
              .Options;

            using (var context = new TasksDbContext(options))
            {
                var userUserRolesService = new UserUserRolesService(null, context);

                User userToAdd = new User
                {
                    Email = "ana@yahoo.com",
                    LastName = "Marcus",
                    FirstName = "Ana",
                    Password = "1234567",
                    RegistrationDate = DateTime.Now,
                    UserUserRoles = new List<UserUserRole>()
                };
                context.Users.Add(userToAdd);

                UserRole addUserRole = new UserRole
                {
                    Name = "Newcomer",
                    Description = "A new guy..."
                };
                context.UserRoles.Add(addUserRole);
                context.SaveChanges();

                context.UserUserRoles.Add(new UserUserRole
                {
                    User = userToAdd,
                    UserRole = addUserRole,
                    StartTime = DateTime.Now,
                    EndTime = null
                });
                context.SaveChanges();

                string userRoleName = userUserRolesService.GetUserRoleNameById(userToAdd.Id);
                Assert.AreEqual("Newcomer", userRoleName);
                Assert.AreEqual("Ana", userToAdd.FirstName);

            }
        }

    }
}
