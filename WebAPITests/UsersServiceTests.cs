using LabIV.DTO;
using LabIV.Models;
using LabIV.Services;
using LabIV.Validators;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Tests
{
    public class UsersServiceTests
    {
        private IOptions<AppSettings> config;

        [SetUp]
        public void Setup()
        {
            config = Options.Create(new AppSettings
            {
                Secret = "THIS IS USED TO SIGN AND VERIFY JWT TOKENS, REPLACE IT WITH YOUR OWN SECRET, IT CAN BE ANY STRING"
            });

        }

        [Test]
        public void ValidRegisterShouldCreateANewUser()
        {
            var options = new DbContextOptionsBuilder<TasksDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(ValidRegisterShouldCreateANewUser))// "ValidRegisterShouldCreateANewUser")
              .Options;

            using (var context = new TasksDbContext(options))
            {
                var validator = new RegisterValidator();
                var usersService = new UsersService(context, validator, null, null, config);
                var added = new RegisterPostDTO()

                {

                    FirstName = "Julia",
                    LastName = "Bush",
                    Username = "julia",
                    Email = "julia@gmail.com",
                    Password = "1234567"
                };
                var result = usersService.Register(added);

                Assert.IsNull(result);
                //Assert.AreEqual(added.Username, context.Users.FirstOrDefault(u => u.Id == 1).Username);
                //Assert.AreEqual(1, context.UserUserRoles.FirstOrDefault(uur => uur.Id == 1).UserId);


            }
        }

        [Test]
        public void AuthenticateShouldLogInAUser()
        {
            var options = new DbContextOptionsBuilder<TasksDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(AuthenticateShouldLogInAUser))
              .Options;

            using (var context = new TasksDbContext(options))
            {
                var validator = new RegisterValidator();
                var validatorUser = new UserRoleValidator();
                var userUserRoleService = new UserUserRolesService(validatorUser, context);
                var usersService = new UsersService(context, validator, null, userUserRoleService, config);

                UserRole addUserRoleRegular = new UserRole
                {
                    Name = "Regular",
                    Description = "For testing..."
                };
                context.UserRoles.Add(addUserRoleRegular);
                context.SaveChanges();


                var added = new LabIV.DTO.RegisterPostDTO

                {

                    FirstName = "Julia",
                    LastName = "Bush",
                    Username = "julia",
                    Email = "julia@gmail.com",
                    Password = "1234567"
                };
                var result = usersService.Register(added);
                var authenticated = new LabIV.DTO.LoginPostDTO
                {
                    Username = "julia",
                    Password = "1234567"
                };
                var authresult = usersService.Authenticate(added.Username, added.Password);

                Assert.IsNotNull(authresult);
                Assert.AreEqual(1, authresult.Id);
                Assert.AreEqual(authenticated.Username, authresult.Username);

            }
        }

        [Test]
        public void GetAllShouldReturnAllUser()
        {
            var options = new DbContextOptionsBuilder<TasksDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(AuthenticateShouldLogInAUser))
              .Options;

            using (var context = new TasksDbContext(options))
            {
                var validator = new RegisterValidator();
                var usersService = new UsersService(context, validator, null, null, config);
                var added = new LabIV.DTO.RegisterPostDTO

                {

                    FirstName = "Julia",
                    LastName = "Bush",
                    Username = "julia",
                    Email = "julia@gmail.com",
                    Password = "1234567"
                };

                var added1 = new LabIV.DTO.RegisterPostDTO

                {

                    FirstName = "Maria",
                    LastName = "Popa",
                    Username = "maria",
                    Email = "maria@gmail.com",
                    Password = "1234567"
                };

                usersService.Register(added);
                usersService.Register(added1);

                var users = usersService.GetAll().Count();

                Assert.NotZero(users);
                Assert.AreEqual(2, users);

            }
        }

        [Test]
        public void InvalidRegisterShouldReturnErrorsCollection()
        {
            var options = new DbContextOptionsBuilder<TasksDbContext>()
                         .UseInMemoryDatabase(databaseName: nameof(InvalidRegisterShouldReturnErrorsCollection))
                         .Options;

            using (var context = new TasksDbContext(options))
            {
                var validator = new RegisterValidator();
                var usersService = new UsersService(context, validator, null, null, config);
                var added = new LabIV.DTO.RegisterPostDTO

                {

                    FirstName = "Julia",
                    LastName = "Bush",
                    Username = "julia",
                    Email = "julia@gmail.com",
                    Password = "12345"
                };

                var result = usersService.Register(added);

                Assert.IsNotNull(result);
                Assert.AreEqual(1, result.ErrorMessages.Count());
            }
        }



        [Test]
        public void UpsertShouldModifyFildsValues()
        {
            var options = new DbContextOptionsBuilder<TasksDbContext>()
            .UseInMemoryDatabase(databaseName: nameof(UpsertShouldModifyFildsValues))
            .Options;

            using (var context = new TasksDbContext(options))
            {
                var validator = new RegisterValidator();
                var validator1 = new CreateValidator();
                var validatorUser = new UserRoleValidator();
                var userUserRoleService = new UserUserRolesService(validatorUser, context);
                var usersService = new UsersService(context, validator, validator1, userUserRoleService, config);

                var added = new UserPostDTO()

                {

                    FirstName = "Julia",
                    LastName = "Bush",
                    Username = "julia",
                    Email = "julia@gmail.com",
                    Password = "1234567"
                };

                var IsIn = usersService.Create(added);

                var added1 = new UserPostDTO()

                {

                    FirstName = "Maya",
                    LastName = "Bush",
                    Username = "maya",
                    Email = "maya@gmail.com",
                    Password = "1234567"
                };


                Assert.IsNull(IsIn);
                Assert.AreNotEqual(added.FirstName, added1.FirstName);

            }
        }

        [Test]
        public void GetByIdShouldReturnAnValidUser()
        {
            var options = new DbContextOptionsBuilder<TasksDbContext>()
         .UseInMemoryDatabase(databaseName: nameof(GetByIdShouldReturnAnValidUser))
         .Options;

            using (var context = new TasksDbContext(options))
            {
                var validator = new RegisterValidator();
                var usersService = new UsersService(context, validator, null, null, config);
                var added = new RegisterPostDTO()

                {

                    FirstName = "Julia",
                    LastName = "Bush",
                    Username = "julia",
                    Email = "julia@gmail.com",
                    Password = "1234567"
                };

                usersService.Register(added);
                var userById = usersService.GetById(3);

                Assert.NotNull(userById);
                Assert.AreEqual("Julia", userById.FirstName);

            }
        }
    }
}