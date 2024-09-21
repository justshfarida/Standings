using Microsoft.EntityFrameworkCore;
using Standings.Domain.Entities.AppDbContextEntity;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Standings.Persistence.Configurations.EntityConfigurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(u => u.UserName).IsRequired().HasMaxLength(50);
            builder.Property(u=>u.Email).IsRequired().HasMaxLength(50);
        }
    }
}
