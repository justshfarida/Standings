using Standings.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Standings.Domain.Entities.AppDbContextEntity
{
    public class Subject:BaseEntity
    {
        public string Name { get; set; }
        public ICollection<Exam> Exams { get; set; }
        public ICollection<GroupSubjects> GroupSubjects { get; set; }
    }
}
