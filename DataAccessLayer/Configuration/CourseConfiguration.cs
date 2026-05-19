using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Configuration
{
    internal class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.ToTable("Courses");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .HasDefaultValueSql("nextval('\"CourseSequence\"')");

            builder.Property(c => c.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.HasOne(c => c.CourseCategory)      
                .WithMany(cc => cc.Courses)           
                .HasForeignKey(c => c.CourseCategoryId) 
                .OnDelete(DeleteBehavior.Restrict);     
        }
    }
}
