using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Standings.Application.DTOS.StudentDTOs
{
    public class StudentUpdateDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int GroupId { get; set; } 
    }
}
