
using System.ComponentModel.DataAnnotations;


namespace Data.Entities;

public class StatusTypeEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = null!;

    // Navigation property for related projects
    public ICollection<ProjectEntity> Projects { get; set; } = new List<ProjectEntity>();
}
