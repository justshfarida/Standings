namespace Standings.Application.DTOS.ExamDTOs
{
    public class ExamGetDTO
    {   
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime ExamDate { get; set; }
        public double Coefficient { get; set; }
        public int SubjectId { get; set; }
    }
}
