using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.Models.Projects;

namespace WebApp.Controllers;

[Authorize]
public class ProjectsController : Controller
{
    public IActionResult Projects()
    {
        var viewModel = new ProjectsViewModel()
        {
            Projects = SetProjects(),
            EditProjectFormData = new EditProjectViewModel
            {
                Statuses = SetStatuses()
            }
        };

        return View(viewModel);
    }

    private IEnumerable<ProjectViewModel>SetProjects()
    {
        var projects = new List<ProjectViewModel>();

        projects.Add(new ProjectViewModel
        {
            Id = Guid.NewGuid().ToString(),
            ProjectName = "Website Redesign",
            ClientName = "GitLab Inc.",
            ProjectDescription = "<span>It is necessary to develop a website redesign in a corporate style.</span>"
        });

        return projects;
    }

    private static IEnumerable<SelectListItem> SetStatuses()
    {
        var status = new List<SelectListItem>
        {
            new SelectListItem { Value = "1", Text = "NOT STARTED", Selected = true },
            new SelectListItem { Value = "2", Text = "STARTED" },
            new SelectListItem { Value = "3", Text = "COMPLETED" },
        };

        return status;
    }
}
