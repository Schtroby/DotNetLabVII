using LabIV.DTO;
using LabIV.Models;
using LabIV.Services;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebAPITests
{
    class UserRoleServiceTests
    {
        [Test]
        public void GetAllShouldReturnAllUserRoles()
        {
            var options = new DbContextOptionsBuilder<TasksDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(GetAllShouldReturnAllUserRoles))
              .Options;

            using (var context = new TasksDbContext(options))
            {
                var userRoleService = new UserRoleService(context);
                var addUserRole = new UserRolePostDTO ()
                {
                    Name = "Newcomer",
                    Description = "A new guy..."
                };


                var current = userRoleService.Create(addUserRole);
                var allUsers = userRoleService.GetAll();
                Assert.IsNotNull(allUsers);
                Assert.AreEqual(1, allUsers.Count());
            }
        }

        [Test]
        public void GetByIdShouldReturnUserRoleWithCorrectId()
        {
            var options = new DbContextOptionsBuilder<TasksDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(GetByIdShouldReturnUserRoleWithCorrectId))
              .Options;

            using (var context = new TasksDbContext(options))
            {
                var userRoleService = new UserRoleService(context);
                var addUserRole = new UserRolePostDTO()
                {
                    Name = "Newcomer",
                    Description = "A new guy..."
                };


                var current = userRoleService.Create(addUserRole);
                var expected = userRoleService.GetById(current.Id);

                Assert.IsNotNull(expected);
                Assert.AreEqual(expected.Name, current.Name);
                Assert.AreEqual(expected.Id, current.Id);
            }
        }

        [Test]
        public void CreateShouldAddAndReturnTheCreatedUserRole()
        {
            var options = new DbContextOptionsBuilder<TasksDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(CreateShouldAddAndReturnTheCreatedUserRole))
              .Options;

            using (var context = new TasksDbContext(options))
            {
                var userRoleService = new UserRoleService(context);
                var addUserRole = new UserRolePostDTO()
                {
                    Name = "Newcomer",
                    Description = "A new guy..."
                };


                var current = userRoleService.Create(addUserRole);
               

                Assert.IsNotNull(current);
                Assert.AreEqual("Newcomer", current.Name);
            }
        }

        [Test]
        public void UpsertShouldModifyTheGivenUserRole()
        {
            var options = new DbContextOptionsBuilder<TasksDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(UpsertShouldModifyTheGivenUserRole))
              .EnableSensitiveDataLogging()
              .Options;

            using (var context = new TasksDbContext(options))
            {
                var userRoleService = new UserRoleService(context);
                var toAdd = new UserRolePostDTO()
                {
                    Name = "Newcomer",
                    Description = "A new guy..."
                };

                var added = userRoleService.Create(toAdd);
                
                var update = new UserRolePostDTO()
                {
                    Name = "Rouge"
                };

                var toUp = userRoleService.Create(update);
                Assert.IsNotNull(toUp);
                Assert.AreEqual(added.Name, added.Name);
                Assert.AreEqual(added.Name, added.Name);


            }
        }

        [Test]
        public void DeleteShouldDeleteAndReturnTheDeletedUserRole()
        {
            var options = new DbContextOptionsBuilder<TasksDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(DeleteShouldDeleteAndReturnTheDeletedUserRole))
              .Options;

            using (var context = new TasksDbContext(options))
            {
                var userRoleService = new UserRoleService(context);
                var addUserRole = new UserRolePostDTO()
                {
                    Name = "Newcomer",
                    Description = "A new guy..."
                };


                var actual = userRoleService.Create(addUserRole);
                var afterDelete = userRoleService.Delete(actual.Id);
                int numberOfUserRoleInDb = context.UserRoles.CountAsync().Result;
                var resultUR = context.UserRoles.Find(actual.Id);


                Assert.IsNotNull(afterDelete);
                Assert.IsNull(resultUR);
                Assert.AreEqual(0, numberOfUserRoleInDb);
            }
        }
    }
}
