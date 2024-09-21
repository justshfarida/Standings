using Standings.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Standings.Domain.Entities.AppDbContextEntity
{
    public class Group:BaseEntity
    {
        public string Name { get; set; }
        public int Year { get; set; }
        //navigation property
        public ICollection<GroupSubjects> GroupSubjects { get; set; }
        public ICollection<Student> Students { get; set; }

    }
}
