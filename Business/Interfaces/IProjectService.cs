
using Business.DTO;

namespace Business.Interfaces;

public interface IProjectService
{
    Task<IEnumerable<ProjectDto>> GetAllAsync();
    Task<ProjectDto?> GetByIdAsync(int id);
    Task AddAsync(AddProjectDto dto);
    Task UpdateAsync(EditProjectDto dto);
    Task DeleteAsync(int id);
    Task<IEnumerable<ClientDto>> GetAllClientsAsync();
    Task<IEnumerable<StatusTypeDto>> GetAllStatusesAsync();
}
