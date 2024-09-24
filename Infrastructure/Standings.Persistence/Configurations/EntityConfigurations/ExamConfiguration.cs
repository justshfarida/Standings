using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Standings.Domain.Entities.AppDbContextEntity;

public class ExamConfiguration : IEntityTypeConfiguration<Exam>
{
    public void Configure(EntityTypeBuilder<Exam> builder)
    {
        builder.HasKey(x => x.Id);
        // Name is required and has a max length of 50
        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(100);

        // ExamDate is required
        builder.Property(e => e.ExamDate)
            .IsRequired();

        // Coefficient is required
        builder.Property(e => e.Coefficient) 
            .IsRequired();

        // Foreign key constraint for Subject
        builder.HasOne(e => e.Subject)
            .WithMany(s => s.Exams)
            .HasForeignKey(e => e.SubjectId);
    }
}
