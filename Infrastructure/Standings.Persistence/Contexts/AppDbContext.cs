using Microsoft.EntityFrameworkCore;
using Standings.Domain.Entities.AppDbContextEntity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public DbSet<Average> Averages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

                
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
            //Configure the many-to-many relationship between Exam and Student
            modelBuilder.Entity<StudentExamResult>().
                HasKey(res=>new {res.StudentId, res.ExamId});
            modelBuilder.Entity<StudentExamResult>().
                HasOne(res=>res.Student).
                WithMany(s => s.Results).
                HasForeignKey(s => s.StudentId);
            modelBuilder.Entity<StudentExamResult>().
                HasOne(res => res.Exam).
                WithMany(ex => ex.Results).
                HasForeignKey(res => res.ExamId);
            //Configure the one-to-many relationship between Student and Average
            modelBuilder.Entity<Average>().
                HasOne(av=>av.Student).
                WithMany(s=>s.Averages).
                HasForeignKey(av => av.StudentId);
            // Seed roles
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Name = "Student", NormalizedName = "STUDENT" },
                new IdentityRole { Name = "Moderator", NormalizedName = "MODERATOR" },
                new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" }
            );

        }
    }

}
