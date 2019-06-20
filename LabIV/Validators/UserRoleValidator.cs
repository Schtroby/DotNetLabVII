using LabIV.DTO;
using LabIV.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LabIV.Validators
{
    public interface IUserRoleValidator
    {
        ErrorsCollection Validate(UserUserRolePostDTO userUserRolePostDTO, TasksDbContext context);
    }

    public class UserRoleValidator : IUserRoleValidator
    {


        public ErrorsCollection Validate(UserUserRolePostDTO userUserRolePostDTO, TasksDbContext context)
        {
            ErrorsCollection errorsCollection = new ErrorsCollection { Entity = nameof(UserUserRolePostDTO) };


            List<string> userRoles = context
                
                .UserRoles
                .Select(userRole => userRole.Name)
                .ToList();


            if (!userRoles.Contains(userUserRolePostDTO.UserRoleName))
            {
                errorsCollection.ErrorMessages.Add($"The UserRole {userUserRolePostDTO.UserRoleName} does not exist!");
            }

            if (errorsCollection.ErrorMessages.Count > 0)
            {
                return errorsCollection;
            }
            return null;
        }

    }
}
