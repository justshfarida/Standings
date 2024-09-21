using Standings.Domain.Entities.Common;

namespace Standings.Domain.Entities.AppDbContextEntity
{
    public class Exam:BaseEntity
    {
        public string Name { get; set; }
        public DateTime ExamDate { get; set; }
        public double Coefficient { get; set; }
        public int SubjectId { get; set; }
        public Subject Subject { get; set; }
        public ICollection<StudentExamResult> Results { get; set; }
    }
}
