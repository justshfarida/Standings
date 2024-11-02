using System.Collections.Generic;
using Standings.Application.DTOS.StudentDTOs;

namespace Standings.Application.DTOS.GroupDTOs
{
    public class GroupGetDTO
    {
        public int Id { get; set; }                   // Group ID
        public string Name { get; set; }              // Group name
        public int Year { get; set; }                 // Academic year

        // Optionally, a list of students in the group
        public List<StudentGetDTO> Students { get; set; } = new List<StudentGetDTO>();
    }
}
