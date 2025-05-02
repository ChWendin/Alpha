using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.Models.Projects;
using Business.Interfaces;
using Business.DTO;

namespace WebApp.Controllers
{
    [Authorize]
    public class ProjectsController : Controller
    {
        private readonly IProjectService _service;

        public ProjectsController(IProjectService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var dto = await _service.GetByIdAsync(id);
            if (dto == null)
                return NotFound();

            var statuses = await _service.GetAllStatusesAsync();

            var vm = new EditProjectViewModel
            {
                Id = dto.Id,
                ProjectName = dto.ProjectName,
                ClientName = dto.ClientName,
                ProjectDescription = dto.ProjectDescription,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Budget = dto.Budget,
                StatusTypeId = dto.StatusTypeId,
                Statuses = statuses
                                        .Select(s => new SelectListItem(s.Name, s.Id.ToString()))
            };


            return PartialView(
                "~/Views/Shared/Modals/_EditProjectModalPartial.cshtml",
                vm
            );
        }
        [HttpGet]
        public async Task<IActionResult> Projects(int? statusTypeId)
        {
            var dtos = await _service.GetAllAsync();

            var all = dtos
                .Select(p => new ProjectViewModel
                {
                    Id = p.Id.ToString(),
                    ProjectName = p.ProjectName,
                    ClientName = p.ClientName,
                    ProjectDescription = p.ProjectDescription ?? "",
                    StartDate = p.StartDate,
                    EndDate = p.EndDate,
                    Budget = p.Budget,
                    StatusText = p.StatusText,
                    StatusTypeId = p.StatusTypeId 
                })
                .ToList();

            var filtered = statusTypeId switch
            {
                2 => all.Where(p => p.StatusTypeId == 2),
                3 => all.Where(p => p.StatusTypeId == 3),
                _ => all
            };

            var statuses = await _service.GetAllStatusesAsync();

            var vm = new ProjectsViewModel
            {
                AllProjects = all,
                Projects = filtered,
                CurrentFilter = statusTypeId ?? 1,

                AddProjectFormData = new AddProjectViewModel
                {
                    Statuses = statuses
                                .Select(s => new SelectListItem(s.Name, s.Id.ToString())),
                    StartDate = DateTime.Today
                },

                EditProjectFormData = new EditProjectViewModel
                {
                    Statuses = statuses
                                .Select(s => new SelectListItem(s.Name, s.Id.ToString())),
                    StartDate = DateTime.Today
                }
            };

            return View(vm);
        }


        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(AddProjectViewModel form)
        {
            if (!ModelState.IsValid)
            {
                ViewData["ShowAddModal"] = true;
                var statuses = await _service.GetAllStatusesAsync();
                form.Statuses = statuses
                    .Select(s => new SelectListItem(s.Name, s.Id.ToString()));

                var vm = await BuildViewModel(form, null);
                return View("Projects", vm);
            }

            await _service.AddAsync(new AddProjectDto
            {
                ProjectName = form.ProjectName,
                ClientName = form.ClientName,
                ProjectDescription = form.ProjectDescription,
                StartDate = form.StartDate,
                EndDate = form.EndDate,
                Budget = form.Budget,
                StatusTypeId = form.StatusTypeId
            });

            return RedirectToAction(nameof(Projects));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditProjectViewModel form)
        {
            if (!ModelState.IsValid)
            {
                ViewData["ShowEditModal"] = true;
                var statuses = await _service.GetAllStatusesAsync();
                form.Statuses = statuses
                    .Select(s => new SelectListItem(s.Name, s.Id.ToString()));

                var vm = await BuildViewModel(null, form);
                return View("Projects", vm);
            }

            await _service.UpdateAsync(new EditProjectDto
            {
                Id = form.Id,
                ProjectName = form.ProjectName,
                ClientName = form.ClientName,
                ProjectDescription = form.ProjectDescription,
                StartDate = form.StartDate,
                EndDate = form.EndDate,
                Budget = form.Budget,
                StatusTypeId = form.StatusTypeId
            });

            return RedirectToAction(nameof(Projects));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Projects));
        }

        private async Task<ProjectsViewModel> BuildViewModel(
            AddProjectViewModel? addForm,
            EditProjectViewModel? editForm)
        {
            var projects = await _service.GetAllAsync();
            var statuses = await _service.GetAllStatusesAsync();

            return new ProjectsViewModel
            {
                Projects = projects.Select(p => new ProjectViewModel
                {
                    Id = p.Id.ToString(),
                    ProjectName = p.ProjectName,
                    ClientName = p.ClientName,
                    ProjectDescription = p.ProjectDescription ?? "",
                    StartDate = p.StartDate,
                    EndDate = p.EndDate,
                    Budget = p.Budget,
                    StatusText = p.StatusText
                }),

                AddProjectFormData = addForm ?? new AddProjectViewModel
                {
                    Statuses = statuses.Select(s => new SelectListItem(s.Name, s.Id.ToString())),
                    StartDate = DateTime.Today
                },

                EditProjectFormData = editForm ?? new EditProjectViewModel
                {
                    Id = editForm?.Id ?? 0,
                    ProjectName = editForm?.ProjectName ?? "",
                    ClientName = editForm?.ClientName ?? "",
                    ProjectDescription = editForm?.ProjectDescription,
                    StartDate = editForm?.StartDate ?? DateTime.Today,
                    EndDate = editForm?.EndDate,
                    Budget = editForm?.Budget ?? 0m,
                    StatusTypeId = editForm?.StatusTypeId ?? 1,
                    Statuses = statuses.Select(s => new SelectListItem(s.Name, s.Id.ToString()))
                }
            };
        }
    }
}
