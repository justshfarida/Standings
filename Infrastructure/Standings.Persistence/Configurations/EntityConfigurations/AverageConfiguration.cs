using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Standings.Domain.Entities.AppDbContextEntity;

public class AverageConfiguration : IEntityTypeConfiguration<Average>
{
    public void Configure(EntityTypeBuilder<Average> builder)
    {
        builder.HasKey(x => x.Id);
        // Year is required
        builder.Property(a => a.Year)
            .IsRequired();

        // StudentId is required
        builder.Property(a => a.StudentId)
            .IsRequired();

        // AverageGrade is required and has a range
        builder.Property(a => a.AverageGrade)
            .IsRequired()
            .HasPrecision(3, 2); // e.g. 99.99

        // Foreign key constraint for Student
        builder.HasOne(a => a.Student)
            .WithMany(s => s.Averages)
            .HasForeignKey(a => a.StudentId);
    }
}
