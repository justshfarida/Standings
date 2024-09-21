using Microsoft.AspNetCore.Identity;
namespace Standings.Domain.Entities.AppDbContextEntity
{
    public class User:IdentityUser<string>
    {
        public Student Student { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenEndTime { get; set; }
    }
}
