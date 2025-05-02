
namespace WebApp.Models.Projects;

public class ProjectsViewModel
{
    // Den lista som loopas i projekt‐kort (filtrerad)
    public IEnumerable<ProjectViewModel> Projects { get; set; } = new List<ProjectViewModel>();

    // Här lägger vi till ALLA projekt, oavsett filter
    public IEnumerable<ProjectViewModel> AllProjects { get; set; } = new List<ProjectViewModel>();

    public AddProjectViewModel AddProjectFormData { get; set; } = new AddProjectViewModel();
    public EditProjectViewModel EditProjectFormData { get; set; } = new EditProjectViewModel();

    // För att markera vilken knapp som är aktiv
    public int CurrentFilter { get; set; } = 1;
}