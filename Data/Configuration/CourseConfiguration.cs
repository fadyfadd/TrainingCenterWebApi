using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configuration
{
    internal class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.ToTable("courses");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("id")
                .HasDefaultValueSql("nextval('\"course_sequence\"')");

            builder.Property(c=>c.CourseCategoryId).HasColumnName("course_category_id")
                .IsRequired();

            builder.Property(c => c.Title).HasColumnName("title")
                .IsRequired()
                .HasMaxLength(200);

            builder.HasOne(c => c.CourseCategory)      
                .WithMany(cc => cc.Courses)           
                .HasForeignKey(c => c.CourseCategoryId) 
                .OnDelete(DeleteBehavior.Restrict);     
        }
    }
}
