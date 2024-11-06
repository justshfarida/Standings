using Microsoft.EntityFrameworkCore;
using Standings.Domain.Entities.AppDbContextEntity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Emit;

namespace Standings.Persistence.Contexts
{
    public class AppDbContext : IdentityDbContext<User, Role, string>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Student> Students { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<StudentExamResult> Results { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<GroupSubjects> GroupSubjects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Student>()
            .Property(s => s.Id)
            .ValueGeneratedNever(); // Prevents auto-generation

            // Configure the many-to-many relationship between Group and Subject
            modelBuilder.Entity<GroupSubjects>()
                .HasKey(gs => new { gs.GroupId, gs.SubjectId });

            modelBuilder.Entity<GroupSubjects>()
                .HasOne(gs => gs.Group)
                .WithMany(g => g.GroupSubjects)
                .HasForeignKey(gs => gs.GroupId);

            modelBuilder.Entity<GroupSubjects>()
                .HasOne(gs => gs.Subject)
                .WithMany(s => s.GroupSubjects)
                .HasForeignKey(gs => gs.SubjectId);
            //Configure the one-to-many relationship between Group and Students
            modelBuilder.Entity<Student>()
                .HasOne(s => s.Group)
                .WithMany(g => g.Students)
                .HasForeignKey(s => s.GroupId);
            modelBuilder.Entity<StudentExamResult>()
            .HasKey(res => new { res.StudentId, res.ExamId }); // Composite Key: StudentId və ExamId

            // StudentExamResult -> Exam əlaqəsi
            modelBuilder.Entity<StudentExamResult>()
                .HasOne(res => res.Exam)
                .WithMany(ex => ex.Results)
                .HasForeignKey(res => res.ExamId)
                .OnDelete(DeleteBehavior.Cascade); // Exam silindikdə onunla əlaqəli nəticələr də silinir

            // StudentExamResult -> Student əlaqəsi
            modelBuilder.Entity<StudentExamResult>()
           .HasKey(r => r.Id);  // Set ResultId as the primary key
            modelBuilder.Entity<StudentExamResult>()
                .HasOne(res => res.Student)
                .WithMany(st => st.Results)
                .HasForeignKey(res => res.StudentId);
        

        }
        //Seed data olaraq bir admin ve bir moderator yaratmaq

        public async Task SeedData(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            // Seed roles
            var guidStudent = Guid.NewGuid().ToString();
            var guidAdmin = Guid.NewGuid().ToString();
            var guidModerator = Guid.NewGuid().ToString();
            if (!await roleManager.RoleExistsAsync("Student"))
            {
                await roleManager.CreateAsync(new Role { Id=guidStudent, Name = "Student", NormalizedName = "STUDENT" });
            }

            if (!await roleManager.RoleExistsAsync("Moderator"))
            {
                await roleManager.CreateAsync(new Role {Id=guidModerator, Name = "Moderator", NormalizedName = "MODERATOR" });
            }

            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new Role {Id=guidAdmin, Name = "Admin", NormalizedName = "ADMIN" });
            }

            // Admin user creation
            var guidAdminUser = Guid.NewGuid().ToString();

            if (await userManager.FindByEmailAsync("admin@example.com") == null)
            {
                var adminUser = new User
                {
                    Id= guidAdminUser,
                    UserName = "admin@example.com",
                    Email = "admin@example.com",
                    SecurityStamp = Guid.NewGuid().ToString(),
                    EmailConfirmed = true,
                    LockoutEnabled = true
                };

                await userManager.CreateAsync(adminUser, "AdminPassword123!");
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }

            // Moderator user creation
            var guidModeratorUser = Guid.NewGuid().ToString();
            if (await userManager.FindByEmailAsync("moderator@example.com") == null)
            {
                var moderatorUser = new User
                {
                    Id= guidModeratorUser,
                    UserName = "moderator@example.com",
                    Email = "moderator@example.com",
                    SecurityStamp = Guid.NewGuid().ToString(),
                    EmailConfirmed = true,
                    LockoutEnabled = true
                };

                await userManager.CreateAsync(moderatorUser, "ModeratorPassword123!");
                await userManager.AddToRoleAsync(moderatorUser, "Moderator");
            }
        }


    }

}
