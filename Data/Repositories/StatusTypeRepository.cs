using Business.Contexts;
using Data.Entities;
using Data.Interfaces;

namespace Data.Repositories
{
    public class StatusTypeRepository
            : BaseRepository<StatusTypeEntity>, IStatusTypeRepository
    {
        public StatusTypeRepository(DataContext context)
            : base(context)
        {
        }
    }
}
