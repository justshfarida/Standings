using Standings.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Standings.Domain.Entities.AppDbContextEntity
{
    public class StudentExamResult:BaseEntity//StudentExamResult
    {
        public int StudentId { get; set; }
        public int ExamId { get; set; }
        public double Grade { get; set; }
        public Student Student { get; set; }
        public Exam Exam { get; set; }
    }
}
