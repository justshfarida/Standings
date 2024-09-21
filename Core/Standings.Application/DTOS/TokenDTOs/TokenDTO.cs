using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Standings.Application.DTOS.TokenDTOs
{
    public class TokenDTO
    {
        public string AccessToken { get; set; }
        public DateTime ExpirationTime { get; set; }//lifetime
        public string RefreshToken { get; set; }
    }
}
