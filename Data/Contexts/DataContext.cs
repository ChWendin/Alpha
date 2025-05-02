using Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Business.Contexts
{
    public class DataContext : IdentityDbContext<MemberEntity>
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        // Exempel på egna entiteter
        public DbSet<ProjectEntity> Projects { get; set; } = null!;
        public DbSet<ClientEntity> Clients { get; set; } = null!;
        public DbSet<StatusTypeEntity> StatusTypes { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Seed status­typer
            builder.Entity<StatusTypeEntity>().HasData(
                new StatusTypeEntity { Id = 1, Name = "NOT STARTED" },
                new StatusTypeEntity { Id = 2, Name = "STARTED" },
                new StatusTypeEntity { Id = 3, Name = "COMPLETED" }
            );
            // Alla nya ProjectEntity får default StatusTypeId = 1
            builder.Entity<ProjectEntity>()
                   .Property(p => p.StatusTypeId)
                   .HasDefaultValue(1);
        }
    }
}