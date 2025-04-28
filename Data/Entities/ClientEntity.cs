using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class ClientEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string ClientName { get; set; } = null!;

        // Navigation property for related projects
        public ICollection<ProjectEntity> Projects { get; set; } = new List<ProjectEntity>();
    }
}

