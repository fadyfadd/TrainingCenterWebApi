using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Configuration
{
    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
             builder.ToTable("students");

             builder.HasKey(s => s.Id);
            builder.Property(s => s.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("nextval('\"student_sequence\"')");

             builder.Property(s => s.FirstName)
                .HasColumnName("first_name")
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(s => s.LastName)
                .HasColumnName("last_name")
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(s => s.Email)
                .HasColumnName("email")
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(s => s.EnrolledAt)
                .HasColumnName("enrolled_at")
                .HasDefaultValueSql("CURRENT_TIMESTAMP"); // Auto-sets date on creation

        }
    }
}
