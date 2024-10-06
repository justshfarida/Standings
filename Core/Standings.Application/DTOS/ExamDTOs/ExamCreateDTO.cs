namespace Standings.Application.DTOS.ExamDTOs
{
    public class ExamCreateDTO
    {
        public string Name { get; set; }
        public DateTime ExamDate { get; set; }
        public double Coefficient { get; set; }
        public string SubjectName { get; set; }
    }
}

