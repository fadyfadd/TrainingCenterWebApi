using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Configuration
{
    internal class CourseCategoryConfiguration : IEntityTypeConfiguration<CourseCategory>
    {
        public void Configure(EntityTypeBuilder<CourseCategory> builder)
        {
             builder.ToTable("courses_categories");
             builder.HasKey(c => c.Id);

             builder.Property(c => c.Id).HasColumnName("id")
                .HasDefaultValueSql("nextval('\"CourseCategorySequence\"')");

             builder.Property(c => c.Name).HasColumnName("name")
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
 
