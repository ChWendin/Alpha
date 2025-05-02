

namespace Business.DTO;

public class AddProjectDto
{
    public string ProjectName { get; set; } = null!;
    public string ClientName { get; set; } = null!;
    public string? ProjectDescription { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public decimal Budget { get; set; }
    public int StatusTypeId { get; set; }
}