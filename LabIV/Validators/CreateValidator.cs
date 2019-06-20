using LabIV.DTO;
using LabIV.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LabIV.Validators
{
    public interface ICreateValidator
    {
        ErrorsCollection Validate(UserPostDTO userPostDTO, TasksDbContext context);
    }

    public class CreateValidator : ICreateValidator
    {
        public ErrorsCollection Validate(UserPostDTO userPostDTO, TasksDbContext context)
        {
            ErrorsCollection errorsCollection = new ErrorsCollection { Entity = nameof(UserPostDTO) };
            User existing = context.Users.FirstOrDefault(u => u.Username == userPostDTO.Username);
            if (existing != null)
            {
                errorsCollection.ErrorMessages.Add($"The username {userPostDTO.Username} is already taken !");
            }
            if (userPostDTO.Password.Length < 7)
            {
                errorsCollection.ErrorMessages.Add("The password cannot be shorter than 7 characters !");
            }
            if (errorsCollection.ErrorMessages.Count > 0)
            {
                return errorsCollection;
            }
            return null;
        }
    }
    
}
