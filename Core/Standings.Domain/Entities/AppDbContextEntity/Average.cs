using Standings.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Standings.Domain.Entities.AppDbContextEntity
{
    public class Average:BaseEntity
    {
        public int Year { get; set; }
        public int StudentId { get; set; }
        public Student Student { get; set; }
        public float AverageGrade { get; set; }
    }
}
