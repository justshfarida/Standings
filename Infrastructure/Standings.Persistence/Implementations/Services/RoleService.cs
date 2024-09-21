using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Standings.Application.Interfaces.IServices;
using Standings.Application.Models.ResponseModels;
using Standings.Domain.Entities.AppDbContextEntity;

namespace Standings.Persistence.Implementations.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<Role> _roleManager;
        public RoleService(RoleManager<Role> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<Response<bool>> CreateRole(string name)
        {
            Response<bool> responseModel = new Response<bool>() { Data=false, StatusCode=400 };
            IdentityResult result =await _roleManager.CreateAsync(new Role { Id=Guid.NewGuid().ToString(), Name=name });
            if (result.Succeeded) { 
                responseModel.Data=true;
                responseModel.StatusCode = 201;
            }
            return responseModel;
        }
        public async Task<Response<bool>> DeleteRole(string id)
        {
            Response<bool> responseModel = new Response<bool>() { Data = false, StatusCode = 400 };
            var role = await _roleManager.FindByIdAsync(id);
            if (role != null)
            {
                IdentityResult result = await _roleManager.DeleteAsync(role);

                if (result.Succeeded)
                {
                    responseModel.Data = true;
                    responseModel.StatusCode = 200;
                }
            }
            return responseModel;
        }

        public async Task<Response<object>> GetAllroles()
        {
            Response<object> responseModel = new Response<object>() { StatusCode = 400, Data = null };
            var roles = await _roleManager.Roles.ToListAsync();
            if (roles.Count() > 0)
            {
                responseModel.Data = roles;
                responseModel.StatusCode = 200;
            }
            return responseModel;
        }

        public async Task<Response<object>> GetRoleById(string id)
        {
            Response<object> responseModel = new Response<object>() { StatusCode = 400, Data = null };
            var role = await _roleManager.FindByIdAsync(id);
            if (role != null)
            {
                responseModel.Data = role;
                responseModel.StatusCode = 200;
            }
            return responseModel;
        }

        public async Task<Response<bool>> UpdateRole(string id, string name)
        {
            Response<bool> responseModel = new Response<bool>() { StatusCode = 400, Data = false };
            var role = await _roleManager.FindByIdAsync(id);
            if (role != null)
            {
                role.Name = name;
                IdentityResult result = await _roleManager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    responseModel.Data = true;
                    responseModel.StatusCode = 200;
                }
            }
            return responseModel;
        }
    }
}
