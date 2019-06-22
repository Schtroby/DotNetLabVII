using LabIV.DTO;
using LabIV.Models;
using LabIV.Services;
using LabIV.Validators;
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
                var validator = new UserRoleValidator();
                var userUserRolesService = new UserUserRolesService(validator, context);

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
                UserRole addUserRole1 = new UserRole
                {
                    Name = "Roug",
                    Description = "A new guy?"
                };
                context.UserRoles.Add(addUserRole);
                context.UserRoles.Add(addUserRole1);
                context.SaveChanges();

                context.UserUserRoles.Add(new UserUserRole
                {
                    User = userToAdd,
                    UserRole = addUserRole,
                    StartTime = DateTime.Now,
                    EndTime = null
                });
                context.SaveChanges();

                var newUUR = new UserUserRolePostDTO
                {
                    UserId = userToAdd.Id,
                    UserRoleName = "Admin"
                };

                var result = userUserRolesService.Create(newUUR);
                Assert.IsNotNull(result);

                var newUUR1 = new UserUserRolePostDTO
                {
                    UserId = userToAdd.Id,
                    UserRoleName = "Admin1"
                };
                var result1 = userUserRolesService.Create(newUUR1);
                Assert.NotNull(result1);


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
