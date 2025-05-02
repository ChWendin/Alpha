using Business.Contexts;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Data.Entities;




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
