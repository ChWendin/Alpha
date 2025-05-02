using Business.DTO;
using Business.Interfaces;
using Data.Entities;
using Data.Interfaces;

namespace Business.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projRepo;
        private readonly IClientRepository _clientRepo;
        private readonly IStatusTypeRepository _statusRepo;

        public ProjectService(
            IProjectRepository projRepo,
            IClientRepository clientRepo,
            IStatusTypeRepository statusRepo)
        {
            _projRepo = projRepo;
            _clientRepo = clientRepo;
            _statusRepo = statusRepo;
        }

        public async Task<IEnumerable<ProjectDto>> GetAllAsync()
        {
            var projects = await _projRepo.GetAllWithDetailsAsync();
            return projects.Select(p => new ProjectDto
            {
                Id = p.Id,
                ProjectName = p.ProjectName,
                ClientName = p.Client.ClientName,
                ProjectDescription = p.ProjectDescription,
                StartDate = p.StartDate,
                EndDate = p.EndDate,
                Budget = p.Budget,
                StatusTypeId = p.StatusTypeId,
                StatusText = p.StatusType.Name
            });
        }

        public async Task<ProjectDto?> GetByIdAsync(int id)
        {
            var p = (await _projRepo.GetAllWithDetailsAsync())
                      .FirstOrDefault(x => x.Id == id);
            if (p == null) return null;

            return new ProjectDto
            {
                Id = p.Id,
                ProjectName = p.ProjectName,
                ClientName = p.Client.ClientName,
                ProjectDescription = p.ProjectDescription,
                StartDate = p.StartDate,
                EndDate = p.EndDate,
                Budget = p.Budget,
                StatusTypeId = p.StatusTypeId,
                StatusText = p.StatusType.Name
            };
        }

        public async Task AddAsync(AddProjectDto dto)
        {
            var client = await _clientRepo.GetAsync(c => c.ClientName == dto.ClientName)
                         ?? new ClientEntity { ClientName = dto.ClientName };
            if (client.Id == 0)
                await _clientRepo.AddAsync(client);

            var p = new ProjectEntity
            {
                ProjectName = dto.ProjectName,
                ClientId = client.Id,
                ProjectDescription = dto.ProjectDescription,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Budget = dto.Budget,
                StatusTypeId = dto.StatusTypeId
            };
            await _projRepo.AddAsync(p);
        }

        public async Task UpdateAsync(EditProjectDto dto)
        {
            var all = await _projRepo.GetAllWithDetailsAsync();
            var p = all.FirstOrDefault(x => x.Id == dto.Id)
                      ?? throw new KeyNotFoundException("Projekt ej funnet");

            var client = await _clientRepo.GetAsync(c => c.ClientName == dto.ClientName)
                         ?? new ClientEntity { ClientName = dto.ClientName };
            if (client.Id == 0)
                await _clientRepo.AddAsync(client);

            p.ProjectName = dto.ProjectName;
            p.ClientId = client.Id;
            p.ProjectDescription = dto.ProjectDescription;
            p.StartDate = dto.StartDate;
            p.EndDate = dto.EndDate;
            p.Budget = dto.Budget;
            p.StatusTypeId = dto.StatusTypeId;

            await _projRepo.UpdateAsync(p);
        }

        public async Task DeleteAsync(int id)
        {
            var all = await _projRepo.GetAllAsync();
            var p = all.FirstOrDefault(x => x.Id == id)
                      ?? throw new KeyNotFoundException("Projekt ej funnet");
            await _projRepo.RemoveAsync(p);
        }

        public async Task<IEnumerable<ClientDto>> GetAllClientsAsync()
        {
            var clients = await _clientRepo.GetAllAsync();
            return clients.Select(c => new ClientDto
            {
                Id = c.Id,
                Name = c.ClientName
            });
        }

        public async Task<IEnumerable<StatusTypeDto>> GetAllStatusesAsync()
        {
            var statuses = await _statusRepo.GetAllAsync();
            return statuses.Select(s => new StatusTypeDto
            {
                Id = s.Id,
                Name = s.Name
            });
        }
    }
}
