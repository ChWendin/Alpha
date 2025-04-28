using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace Data.Repositories
{
    public class ProjectRepository
            : BaseRepository<ProjectEntity>, IProjectRepository
    {
        public ProjectRepository(DataContext context)
            : base(context)
        {
        }

        public async Task<IEnumerable<ProjectEntity>> GetAllWithDetailsAsync()
        {
            return await _context.Set<ProjectEntity>()
                                 .Include(p => p.Client)
                                 .Include(p => p.StatusType)
                                 .ToListAsync();
        }
    }
}
