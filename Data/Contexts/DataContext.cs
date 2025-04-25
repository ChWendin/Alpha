

using Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data.Contexts
{
    public class DataContext : IdentityDbContext<MemberEntity>
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        // Exempel på egna entiteter


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // här kan du konfigurera ytterligare din modell
        }
    }
}