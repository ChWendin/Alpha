using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Models.Projects
{
    public class EditProjectViewModel
    {
        public IEnumerable<SelectListItem> Clients { get; set; } = [];
        public IEnumerable<SelectListItem> Menbers { get; set; } = [];

        public IEnumerable<SelectListItem> Statuses { get; set; } = [];
    }
}
