using LabIV.DTO;
using LabIV.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LabIV.Services
{
    public interface IUserRoleService
    {
        UserRoleGetDTO GetById(int id);
        UserRoleGetDTO Delete(int id);
        IEnumerable<UserRoleGetDTO> GetAll();
        UserRoleGetDTO Create(UserRolePostDTO userRolePostDTO);
        UserRoleGetDTO Upsert(int id, UserRolePostDTO userRolePostDTO);
        
    }

    public class UserRoleService : IUserRoleService
    {
        private TasksDbContext context;

        public UserRoleService(TasksDbContext context)
        {
            this.context = context;
        }
                

        public UserRoleGetDTO GetById(int id)
        {
            UserRole userRole = context
                .UserRoles
                .AsNoTracking()
                .FirstOrDefault(urole => urole.Id == id);

            return UserRoleGetDTO.FromUserRole(userRole);
        }

        public UserRoleGetDTO Delete(int id)
        {
            var existing = context
                .UserRoles
                .FirstOrDefault(urole => urole.Id == id);
            if (existing == null)
            {
                return null;
            }

            context.UserRoles.Remove(existing);
            context.SaveChanges();

            return UserRoleGetDTO.FromUserRole(existing);
        }


        public UserRoleGetDTO Create(UserRolePostDTO userRolePostDTO)
        {
            UserRole toAdd = UserRolePostDTO.ToUserRole(userRolePostDTO);

            context.UserRoles.Add(toAdd);
            context.SaveChanges();
            return UserRoleGetDTO.FromUserRole(toAdd);
        }


        public UserRoleGetDTO Upsert(int id, UserRolePostDTO userRolePostDTO)
        {
            var existing = context.UserRoles.AsNoTracking().FirstOrDefault(urole => urole.Id == id);
            if (existing == null)
            {
                UserRole toAdd = UserRolePostDTO.ToUserRole(userRolePostDTO);
                context.UserRoles.Add(toAdd);
                context.SaveChanges();
                return UserRoleGetDTO.FromUserRole(toAdd);
            }

            UserRole Up = UserRolePostDTO.ToUserRole(userRolePostDTO);
            Up.Id = id;
            context.UserRoles.Update(Up);
            context.SaveChanges();
            return UserRoleGetDTO.FromUserRole(Up);
        }
               
        public IEnumerable<UserRoleGetDTO> GetAll()
        {
            return context.UserRoles.Select(userRol => UserRoleGetDTO.FromUserRole(userRol));
        }

    }
}
