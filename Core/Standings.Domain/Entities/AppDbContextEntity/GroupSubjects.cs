using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Standings.Domain.Entities.AppDbContextEntity
{
    public class GroupSubjects
    {
        public int GroupId { get; set; }
        public Group Group { get; set; }
        public int SubjectId{ get; set; }
        public Subject Subject { get; set; }
    }
}
