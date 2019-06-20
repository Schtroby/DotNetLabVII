using LabIV.DTO;
using LabIV.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LabIV.Validators
{
    public interface IRegisterValidator
    {
        ErrorsCollection Validate(RegisterPostDTO registerPostDTO, TasksDbContext context);
    }

    public class RegisterValidator : IRegisterValidator
    {
        public ErrorsCollection Validate(RegisterPostDTO registerPostDTO, TasksDbContext context)
        {
            ErrorsCollection errorsCollection = new ErrorsCollection { Entity = nameof(RegisterPostDTO) };
            User existing = context.Users.FirstOrDefault(u => u.Username == registerPostDTO.Username);
            if (existing != null)
            {
                errorsCollection.ErrorMessages.Add($"The username {registerPostDTO.Username} is already taken !");
            }
            if (registerPostDTO.Password.Length < 7)
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
