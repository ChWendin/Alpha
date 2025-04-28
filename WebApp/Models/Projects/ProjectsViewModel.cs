using System.Collections.Generic;

namespace WebApp.Models.Projects
{
    public class ProjectsViewModel
    {

        public IEnumerable<ProjectViewModel> Projects { get; set; } = new List<ProjectViewModel>();

        public AddProjectViewModel AddProjectFormData { get; set; } = new AddProjectViewModel();

        public EditProjectViewModel EditProjectFormData { get; set; } = new EditProjectViewModel();
    }
}