using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer
{
    public class MainDataBaseContext : IdentityDbContext<
        ApplicationUser,
        ApplicationRole,
        int>

    {

        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);

            builder.HasSequence<int>("CourseCategorySequence")
             .StartsAt(1000)
             .IncrementsBy(1);
            
            builder.HasSequence<int>("CourseSequence")
                .StartsAt(5000)
                .IncrementsBy(1);

            builder.ApplyConfigurationsFromAssembly(typeof(MainDataBaseContext).Assembly);
        }
        public MainDataBaseContext(DbContextOptions<MainDataBaseContext> options): base(options)
        {
          
        }
    }
}
