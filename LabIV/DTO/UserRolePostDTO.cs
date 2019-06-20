using LabIV.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LabIV.DTO
{
    public class UserRolePostDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public static UserRole ToUserRole(UserRolePostDTO userRolePostDTO)
        {
            return new UserRole
            {
                Name = userRolePostDTO.Name,
                Description = userRolePostDTO.Description
            };
        }
    }
}
