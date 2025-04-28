using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models.Projects
{
    public class AddProjectViewModel
    {
        [Required]
        [Display(Name = "Project Name")]
        public string ProjectName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Client Name")]
        public string ClientName { get; set; } = string.Empty;

        [Display(Name = "Description")]
        public string? ProjectDescription { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; } = DateTime.Today;

        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        public DateTime? EndDate { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        [Display(Name = "Budget")]
        public decimal Budget { get; set; }

        [Required]
        [Display(Name = "Status")]
        public int StatusTypeId { get; set; } = 1;


        public IEnumerable<SelectListItem> Statuses { get; set; } = new List<SelectListItem>();
        }
    }