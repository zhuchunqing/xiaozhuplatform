using BisPlatform.Data.Entity;
using BisPlatform.Data.Entity.Platform;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BisPlatform.Data.Mapping.Platform
{
    public class TestMap: IEntityTypeConfiguration<TestEntity>
    {
        public void Configure(EntityTypeBuilder<TestEntity> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Name).HasMaxLength(200);
            builder.ToTable("xiaoma");
            builder.Property(a => a.Score).HasMaxLength(12).IsRequired();
            builder.HasOne(a => a.test2Entity).WithOne(b => b.testEntity).HasForeignKey<Test2Entity>(a => a.TestId);
        }
    }
}
