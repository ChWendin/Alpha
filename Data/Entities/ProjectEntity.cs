using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class ProjectEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string ProjectName { get; set; } = null!;

        [Column(TypeName = "text")]
        public string? ProjectDescription { get; set; }

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Budget { get; set; }

        // Foreign key to Client
        public int ClientId { get; set; }
        public ClientEntity Client { get; set; } = null!;

        // Foreign key to StatusType
        public int StatusTypeId { get; set; }
        public StatusTypeEntity StatusType { get; set; } = null!;
    }
}
