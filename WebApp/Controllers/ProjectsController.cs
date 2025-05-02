using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.Models.Projects;
using Business.Interfaces;        // där IProjectService ligger
using Business.DTO;              // AddProjectDto, EditProjectDto, ProjectDto

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
            // 1) Hämta den enskilda DTO:n
            var dto = await _service.GetByIdAsync(id);
            if (dto == null)
                return NotFound();

            // 2) Hämta status-listan
            var statuses = await _service.GetAllStatusesAsync();

            // 3) Mappa till din vy-modell
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

            // 4) Returnera just partialen
            return PartialView(
                "~/Views/Shared/Modals/_EditProjectModalPartial.cshtml",
                vm
            );
        }
        [HttpGet]
        public async Task<IActionResult> Projects(int? statusTypeId)
        {
            // 1) Hämta alla DTO:er från service-lagret
            var dtos = await _service.GetAllAsync();

            // 2) Mappa till ProjectViewModel (inkl. StatusTypeId för filtrering)
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
                    StatusTypeId = p.StatusTypeId      // <— för att kunna filtrera
                })
                .ToList();

            // 3) Filtrera listan baserat på statusTypeId
            var filtered = statusTypeId switch
            {
                2 => all.Where(p => p.StatusTypeId == 2),
                3 => all.Where(p => p.StatusTypeId == 3),
                _ => all
            };

            // 4) Hämta alla statusar för dropdowns i modalerna
            var statuses = await _service.GetAllStatusesAsync();

            // 5) Bygg upp vy-modellen
            var vm = new ProjectsViewModel
            {
                AllProjects = all,
                Projects = filtered,
                CurrentFilter = statusTypeId ?? 1,

                // Formdata för "Add Project"-modalen
                AddProjectFormData = new AddProjectViewModel
                {
                    Statuses = statuses
                                .Select(s => new SelectListItem(s.Name, s.Id.ToString())),
                    StartDate = DateTime.Today
                },

                // Formdata för "Edit Project"-modalen (tomt initialt, fylls via AJAX)
                EditProjectFormData = new EditProjectViewModel
                {
                    Statuses = statuses
                                .Select(s => new SelectListItem(s.Name, s.Id.ToString())),
                    StartDate = DateTime.Today
                }
            };

            return View(vm);
        }


        // POST /Projects/Add
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

        // POST /Projects/Edit
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

        // POST /Projects/Delete/{id}
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Projects));
        }

        // Bygg om hela vy-modellen när Add/Edit validering misslyckas
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
