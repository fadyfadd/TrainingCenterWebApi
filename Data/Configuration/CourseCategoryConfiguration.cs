using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configuration
{
    internal class CourseCategoryConfiguration : IEntityTypeConfiguration<CourseCategory>
    {
        public void Configure(EntityTypeBuilder<CourseCategory> builder)
        {
             builder.ToTable("courses_categories");
             builder.HasKey(c => c.Id);

             builder.Property(c => c.Id).HasColumnName("id")
                .HasDefaultValueSql("nextval('\"course_category_sequence\"')");

             builder.Property(c => c.Name).HasColumnName("name")
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
 
