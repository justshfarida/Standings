using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Standings.Domain.Entities.AppDbContextEntity;

public class GroupSubjectsConfiguration : IEntityTypeConfiguration<GroupSubjects>
{
    public void Configure(EntityTypeBuilder<GroupSubjects> builder)
    {
        builder.HasKey(gs => new { gs.GroupId, gs.SubjectId });

        builder.HasOne(gs => gs.Group)
            .WithMany(g => g.GroupSubjects)
            .HasForeignKey(gs => gs.GroupId);

        builder.HasOne(gs => gs.Subject)
            .WithMany(s => s.GroupSubjects)
            .HasForeignKey(gs => gs.SubjectId);
    }
}
