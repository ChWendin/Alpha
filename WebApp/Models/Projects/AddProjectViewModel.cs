using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Models.Projects;

public class AddProjectViewModel
{
    public IEnumerable<SelectListItem> Clients { get; set; } = [];
    public IEnumerable<SelectListItem> Menbers { get; set; } = [];


}
