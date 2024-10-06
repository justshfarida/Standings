using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Standings.Application.DTOS.StudentDTOs
{
    public class StudentCreateDTO
    {
        public int Id {  get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string GroupName { get; set; }
        public int GroupYear { get; set; }
        public string Email {  get; set; }  
        public string? UserId { get; set; } 

    }
}
