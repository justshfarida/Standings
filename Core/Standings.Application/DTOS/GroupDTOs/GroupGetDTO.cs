using System.Collections.Generic;
using Standings.Application.DTOS.StudentDTOs;

namespace Standings.Application.DTOS.GroupDTOs
{
    public class GroupGetDTO
    {
        public int Id { get; set; }                   
        public string Name { get; set; }              
        public int Year { get; set; }                 
    }
}
