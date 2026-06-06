using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
 
namespace Data
{
    public class MainDataBaseContext : IdentityDbContext<
        ApplicationUser,
        ApplicationRole,
        int>

    {

        public DbSet<CourseCategory> CourseCategories { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Student> Students { get; set; }

        public DbSet<Administrator> Administrators { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>(b => b.ToTable("application_users"));
            builder.Entity<ApplicationRole>(b => b.ToTable("application_roles"));
            builder.Entity<IdentityUserRole<int>>(b => b.ToTable("application_user_roles"));
            builder.Entity<IdentityUserClaim<int>>(b => b.ToTable("application_user_claims"));
            builder.Entity<IdentityRoleClaim<int>>(b => b.ToTable("application_role_claims"));
            builder.Entity<IdentityUserLogin<int>>(b => b.ToTable("application_user_logins"));
            builder.Entity<IdentityUserToken<int>>(b => b.ToTable("application_user_tokens"));

            builder.Entity<ApplicationUser>()
            .Property(u => u.Email)
            .IsRequired();

            builder.HasSequence<int>("course_category_sequence")
             .StartsAt(1)
             .IncrementsBy(1);

            builder.HasSequence<int>("course_sequence")
                .StartsAt(1)
                .IncrementsBy(1);

            builder.HasSequence<int>("administarator_sequence").StartsAt(1).IncrementsBy(1);

            builder.HasSequence<int>("student_sequence")
           .StartsAt(1)
           .IncrementsBy(1);

            builder.ApplyConfigurationsFromAssembly(typeof(MainDataBaseContext).Assembly);
        }
        public MainDataBaseContext(DbContextOptions<MainDataBaseContext> options) : base(options)
        {

        }
    }
}
