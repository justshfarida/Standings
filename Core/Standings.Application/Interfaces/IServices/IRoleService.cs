using Standings.Application.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Standings.Application.Interfaces.IServices
{
    public interface IRoleService
    {
        Task<Response<object>> GetAllroles();
        Task<Response<object>> GetRoleById(string id);
        Task<Response<bool>> CreateRole(string name);
        Task<Response<bool>> DeleteRole(string id);
        Task<Response<bool>> UpdateRole(string id, string name);
    }
}
