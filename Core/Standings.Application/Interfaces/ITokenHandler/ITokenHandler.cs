using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Standings.Application.DTOS.TokenDTOs;
using Standings.Domain.Entities.AppDbContextEntity;

namespace Standings.Application.Interfaces.ITokenHandler
{
    public interface ITokenHandler
    {
        Task<TokenDTO> CreateToken(User user);

        string CreateRefreshToken();
    }
}
