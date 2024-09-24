using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Standings.Domain.Entities.AppDbContextEntity;

public class StudentExamResultConfiguration : IEntityTypeConfiguration<StudentExamResult>
{
    public void Configure(EntityTypeBuilder<StudentExamResult> builder)
    {
        builder.Property(s => s.Grade)
            .IsRequired();

        builder.HasOne(s => s.Student)
            .WithMany(s => s.Results)
            .HasForeignKey(s => s.StudentId);

        builder.HasOne(s => s.Exam)
            .WithMany(e => e.Results)
            .HasForeignKey(s => s.ExamId);
    }
}
