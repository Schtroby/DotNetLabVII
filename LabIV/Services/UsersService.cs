using LabIV.Constants;
using LabIV.DTO;
using LabIV.Models;
using LabIV.Validators;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Task = LabIV.Models.Task;


namespace LabIV.Services
{
    public interface IUsersService
    {
        LogInGetDTO Authenticate(string username, string password);
        ErrorsCollection Register(RegisterPostDTO registerInfo);
        User GetCurrentUser(HttpContext httpContext);
        ErrorsCollection Create(UserPostDTO createInfo);
        User Delete(int id);
        User GetById(int id);
        User Upsert(int id, User user);
        IEnumerable<UserGetDTO> GetAll();
    }

    public class UsersService : IUsersService
    {
        private TasksDbContext context;
        private readonly AppSettings appSettings;
        private IRegisterValidator registerValidator;
        private ICreateValidator createValidator;
        private IUserUserRolesService userUserRolesService;

        public UsersService(TasksDbContext context, IRegisterValidator registerValidator, ICreateValidator createValidator, IUserUserRolesService userUserRolesService, IOptions<AppSettings> appSettings)
        {
            this.context = context;
            this.appSettings = appSettings.Value;
            this.registerValidator = registerValidator;
            this.userUserRolesService = userUserRolesService;
            this.createValidator = createValidator;
        }

        public LogInGetDTO Authenticate(string username, string password)
        {
            var user = context.Users
                .SingleOrDefault(x => x.Username == username &&
                                 x.Password == ComputeSha256Hash(password));

            string userRoleName = userUserRolesService.GetUserRoleNameById(user.Id);

            // return null if user not found
            if (user == null)
                return null;

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Username.ToString()),
                    new Claim(ClaimTypes.Role, userRoleName.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var result = new LogInGetDTO
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.Username,
                Token = tokenHandler.WriteToken(token)
            };
            // remove password before returning
            return result;
        }

        private string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            // TODO: also use salt
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public ErrorsCollection Register(RegisterPostDTO registerInfo)
        {
            var errors = registerValidator.Validate(registerInfo, context);
            if (errors != null)
            {
                return errors;
            }

            User toAdd = new User
            {
                FirstName = registerInfo.FirstName,
                LastName = registerInfo.LastName,
                Email = registerInfo.Email,
                Username = registerInfo.Username,
                Password = ComputeSha256Hash(registerInfo.Password),
                UserUserRoles = new List<UserUserRole>()
            };

            var defaultRole = context
               .UserRoles
               .FirstOrDefault(urole => urole.Name == UserRoles.Regular);

           

            context.Users.Add(toAdd);
            context.UserUserRoles.Add(new UserUserRole
            {
                User = toAdd,
                UserRole = defaultRole,
                StartTime = DateTime.Now,
                EndTime = null

            });

            context.SaveChanges();
            return null;
        }

        public UserRole GetCurrentUserRole(User user)
        {
            return user
                .UserUserRoles
                .FirstOrDefault(userUserRole => userUserRole.EndTime == null)
                .UserRole;
        }

        public User GetCurrentUser(HttpContext httpContext)
        {
            string username = httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;
            //string accountType = httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.AuthenticationMethod).Value;
            //return _context.Users.FirstOrDefault(u => u.Username == username && u.AccountType.ToString() == accountType);
            return context.Users
                 .Include(u => u.UserUserRoles)
                 .FirstOrDefault(u => u.Username == username);
        }

        public IEnumerable<UserGetDTO> GetAll()
        {
            // return users without passwords
            return context.Users.Select(user => new UserGetDTO
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.Username,
                //Role = user.UserRole
            });
        }

        public ErrorsCollection Create (UserPostDTO createInfo)
        {
            var errors = createValidator.Validate(createInfo, context);
            if (errors != null)
            {
                return errors;
            }

            User toAdd = new User
            {
                FirstName = createInfo.FirstName,
                LastName = createInfo.LastName,
                Email = createInfo.Email,
                Username = createInfo.Username,
                Password = ComputeSha256Hash(createInfo.Password),
                UserUserRoles = new List<UserUserRole>()
            };

            var defaultRole = context
               .UserRoles
               .FirstOrDefault(urole => urole.Name == UserRoles.Regular);



            context.Users.Add(toAdd);
            context.UserUserRoles.Add(new UserUserRole
            {
                User = toAdd,
                UserRole = defaultRole,
                StartTime = DateTime.Now,
                EndTime = null

            });

            context.SaveChanges();
            return null;
        }

        //public User Create(UserPostDTO user)
        //{
        //    User userAdd = UserPostDTO.ToUser(user);
        //    context.Users.Add(userAdd);
        //    context.SaveChanges();
        //    return userAdd;
        //}

        public User Delete(int id)
        {
            var existing = context.Users.FirstOrDefault(user => user.Id == id);
            if (existing == null)
            {
                return null;
            }
                 context.Users
                .Remove(existing);
                context.SaveChanges();
            return existing;
        }

        public User GetById(int id)
        {
            return context.Users
                .AsNoTracking()
                .FirstOrDefault(u => u.Id == id);
        }

        public User Upsert(int id, User user)
        {
            var existing = context.Users.AsNoTracking().FirstOrDefault(u => u.Id == id);
            if (existing == null)
            {
                //User UserAdd = User.ToUser(user);
                user.Password = ComputeSha256Hash(user.Password);
                context.Users.Add(user);
                context.SaveChanges();
                return user;

            }
            //User UserUp = User.ToUser(user);
            user.Id = id;
            user.Password = ComputeSha256Hash(user.Password);
            context.Users.Update(user);
            context.SaveChanges();
            return user;
        }

    }

}
