using Standings.Domain.Entities.Common;

namespace Standings.Domain.Entities.AppDbContextEntity
{
    public class Student:BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int GroupId { get; set; }
        public Group Group { get; set; }
        public ICollection<StudentExamResult> Results { get; set; }
        public string UserId { get; set; } // Ensure this matches User.Id type
        public User User { get; set; }
        public ICollection<Average> Averages { get; set; }
        //Bitirib ya bitirmeyib unini 
        //Gpa i 
    }
}
