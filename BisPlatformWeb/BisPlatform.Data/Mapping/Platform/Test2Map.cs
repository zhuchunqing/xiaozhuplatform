using BisPlatform.Data.Entity;
using BisPlatform.Data.Entity.Platform;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BisPlatform.Data.Mapping.Platform
{
    public class Test2Map : IEntityTypeConfiguration<Test2Entity>
    {
        public void Configure(EntityTypeBuilder<Test2Entity> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.BIM).HasMaxLength(12);
            builder.ToTable("xiaoma2");
            builder.Property(a => a.Height).HasMaxLength(12).IsRequired();
        }
    }
}
