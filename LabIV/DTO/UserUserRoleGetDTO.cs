using LabIV.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LabIV.DTO
{


    public class UserUserRoleGetDTO
    {
        
        public int Id { get; set; }
        public int UserId { get; set; }
        public int UserRoleId { get; set; }
        public string UserRoleName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }



        public static UserUserRoleGetDTO FromUserUserRole(UserUserRole userUserRole)
        {

            return new UserUserRoleGetDTO
            {

                Id = userUserRole.Id,
                UserId = userUserRole.UserId,
                UserRoleId = userUserRole.UserRoleId,
                UserRoleName = userUserRole.UserRole.Name,
                StartTime = userUserRole.StartTime,
                EndTime = userUserRole.EndTime
            };
        }
    }
}
