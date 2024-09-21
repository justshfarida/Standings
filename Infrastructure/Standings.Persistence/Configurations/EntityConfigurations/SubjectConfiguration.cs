using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Standings.Domain.Entities.AppDbContextEntity;

public class SubjectConfiguration : IEntityTypeConfiguration<Subject>
{
    public void Configure(EntityTypeBuilder<Subject> builder)
    {
        builder.HasKey(s=>s.Id);
        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasMany(s => s.Exams)
            .WithOne(e => e.Subject)
            .HasForeignKey(e => e.SubjectId);

        builder.HasMany(s => s.GroupSubjects)
            .WithOne(gs => gs.Subject)
            .HasForeignKey(gs => gs.SubjectId);
    }
}
