

using Data.Contexts;
using Data.Entities;
using Data.Interfaces;


namespace Data.Repositories
{
    public class ClientRepository
        : BaseRepository<ClientEntity>, IClientRepository
    {
        public ClientRepository(DataContext context)
            : base(context)
        {
        }
    }
}
