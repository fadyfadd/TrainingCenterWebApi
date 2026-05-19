using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Configuration
{
    internal class CourseCategoryConfiguration
    {
        public void Configure(EntityTypeBuilder<CourseCategory> builder)
        {
             builder.ToTable("CourseCategories");
             builder.HasKey(c => c.Id);

             builder.Property(c => c.Id)
                .HasDefaultValueSql("nextval('\"CourseCategorySequence\"')");

             builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
 
