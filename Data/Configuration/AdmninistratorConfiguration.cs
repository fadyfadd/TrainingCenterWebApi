using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Data.Configuration
{
    public class AdmninistratorConfiguration : IEntityTypeConfiguration<Administrator>
    {
        void IEntityTypeConfiguration<Administrator>.Configure(EntityTypeBuilder<Administrator> builder)
        {


            builder.ToTable("administrators");

            builder.HasKey(s => s.Id);
            builder.Property(s => s.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("nextval('\"administarator_sequence\"')");

            builder.Property(s => s.FirstName)
               .HasColumnName("first_name")
               .IsRequired()
               .HasMaxLength(50);

            builder.Property(s => s.UserId)
                .HasColumnName("user_id")
                .IsRequired();

            builder.Property(s => s.LastName)
                .HasColumnName("last_name")
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(s => s.EnrolledAt)
                .HasColumnName("enrolled_at")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.HasOne(s => s.User)
                .WithOne()
                .HasForeignKey<Administrator>(s => s.UserId)
                .HasPrincipalKey<ApplicationUser>(u => u.Id)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
