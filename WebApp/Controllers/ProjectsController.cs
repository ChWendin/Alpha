using Microsoft.AspNetCore.Mvc;
using WebApp.Models.Projects;

namespace WebApp.Controllers;

public class ProjectsController : Controller
{
    public IActionResult Projects()
    {
        var viewModel = new ProjectsViewModel()
        {
            Projects = SetProjects()
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
}
