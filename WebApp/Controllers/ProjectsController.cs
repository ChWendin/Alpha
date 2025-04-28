using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Data.Entities;
using Data.Interfaces;
using WebApp.Models.Projects;

namespace WebApp.Controllers
{
    [Authorize]
    public class ProjectsController : Controller
    {
        private readonly IProjectRepository _projectRepo;
        private readonly IClientRepository _clientRepo;
        private readonly IStatusTypeRepository _statusRepo;

        public ProjectsController(
            IProjectRepository projectRepo,
            IClientRepository clientRepo,
            IStatusTypeRepository statusRepo)
        {
            _projectRepo = projectRepo;
            _clientRepo = clientRepo;
            _statusRepo = statusRepo;
        }

        // GET /Projects/Edit/5 – returnerar bara formuläret (partial) för AJAX
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            // Hämta med Client och StatusType inkluderade
            var all = await _projectRepo.GetAllWithDetailsAsync();
            var p = all.FirstOrDefault(x => x.Id == id);
            if (p == null) return NotFound();

            var statuses = await _statusRepo.GetAllAsync();

            var vm = new EditProjectViewModel
            {
                Id = p.Id,
                ProjectName = p.ProjectName,
                ClientName = p.Client.ClientName,
                ProjectDescription = p.ProjectDescription,
                StartDate = p.StartDate,
                EndDate = p.EndDate,
                Budget = p.Budget,
                StatusTypeId = p.StatusTypeId,
                Statuses = statuses
                                       .Select(s => new SelectListItem(s.Name, s.Id.ToString()))
            };

            return PartialView(
            "~/Views/Shared/Modals/_EditProjectModalPartial.cshtml", vm);
        }

        // GET /Projects
        [HttpGet]
        public async Task<IActionResult> Projects()
        {
            var projects = await _projectRepo.GetAllWithDetailsAsync();
            var statuses = await _statusRepo.GetAllAsync();

            var vm = new ProjectsViewModel
            {
                Projects = projects.Select(p => new ProjectViewModel
                {
                    Id = p.Id.ToString(),
                    ProjectName = p.ProjectName,
                    ClientName = p.Client.ClientName,
                    ProjectDescription = p.ProjectDescription ?? string.Empty,
                    StartDate = p.StartDate,
                    EndDate = p.EndDate,
                    Budget = p.Budget,
                    StatusText = p.StatusType.Name
                }),

                AddProjectFormData = new AddProjectViewModel
                {
                    Statuses = statuses
                        .Select(s => new SelectListItem(s.Name, s.Id.ToString()))
                },

                // Initiera en tom EditFormData – AJAX kommer ladda in värden senare
                EditProjectFormData = new EditProjectViewModel
                {
                    Statuses = statuses
                        .Select(s => new SelectListItem(s.Name, s.Id.ToString()))
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
                var statuses = await _statusRepo.GetAllAsync();
                form.Statuses = statuses
                    .Select(s => new SelectListItem(s.Name, s.Id.ToString()));

                var vm = await BuildProjectsViewModel(form, null);
                return View("Projects", vm);
            }

            // Klienthantering
            var client = await _clientRepo.GetAsync(c => c.ClientName == form.ClientName);
            if (client == null)
            {
                client = new ClientEntity { ClientName = form.ClientName };
                await _clientRepo.AddAsync(client);
            }

            var project = new ProjectEntity
            {
                ProjectName = form.ProjectName,
                ClientId = client.Id,
                ProjectDescription = form.ProjectDescription,
                StartDate = form.StartDate,
                EndDate = form.EndDate,
                Budget = form.Budget,
                StatusTypeId = form.StatusTypeId
            };
            await _projectRepo.AddAsync(project);

            return RedirectToAction(nameof(Projects));
        }

        // POST /Projects/Delete/5
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var project = await _projectRepo.GetAsync(p => p.Id == id);
            if (project == null) return NotFound();

            await _projectRepo.RemoveAsync(project);
            return RedirectToAction(nameof(Projects));
        }

        // POST /Projects/Edit (saves)
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditProjectViewModel form)
        {
            if (!ModelState.IsValid)
            {
                var statuses = await _statusRepo.GetAllAsync();
                form.Statuses = statuses
                    .Select(s => new SelectListItem(s.Name, s.Id.ToString()));

                var vm = await BuildProjectsViewModel(null, form);
                return View("Projects", vm);
            }

            // Klienthantering om namn ändrats
            var client = await _clientRepo.GetAsync(c => c.ClientName == form.ClientName);
            if (client == null)
            {
                client = new ClientEntity { ClientName = form.ClientName };
                await _clientRepo.AddAsync(client);
            }

            var project = new ProjectEntity
            {
                Id = form.Id,
                ProjectName = form.ProjectName,
                ClientId = client.Id,
                ProjectDescription = form.ProjectDescription,
                StartDate = form.StartDate,
                EndDate = form.EndDate,
                Budget = form.Budget,
                StatusTypeId = form.StatusTypeId
            };
            await _projectRepo.UpdateAsync(project);

            return RedirectToAction(nameof(Projects));
        }

        // Hjälpmetod för Add/Edit‐fel: bygger om hela vymodellen
        private async Task<ProjectsViewModel> BuildProjectsViewModel(
            AddProjectViewModel? addForm,
            EditProjectViewModel? editForm)
        {
            // Använd detaljerad hämtning här också
            var projects = await _projectRepo.GetAllWithDetailsAsync();
            var statuses = await _statusRepo.GetAllAsync();

            return new ProjectsViewModel
            {
                Projects = projects.Select(p => new ProjectViewModel
                {
                    Id = p.Id.ToString(),
                    ProjectName = p.ProjectName,
                    ClientName = p.Client.ClientName,
                    ProjectDescription = p.ProjectDescription ?? string.Empty,
                    StartDate = p.StartDate,
                    EndDate = p.EndDate,
                    Budget = p.Budget,
                    StatusText = p.StatusType.Name
                }),

                AddProjectFormData = addForm ?? new AddProjectViewModel
                {
                    Statuses = statuses
                        .Select(s => new SelectListItem(s.Name, s.Id.ToString()))
                },

                EditProjectFormData = editForm ?? new EditProjectViewModel
                {
                    Id = editForm?.Id ?? 0,
                    ProjectName = editForm?.ProjectName ?? string.Empty,
                    ClientName = editForm?.ClientName ?? string.Empty,
                    ProjectDescription = editForm?.ProjectDescription,
                    StartDate = editForm?.StartDate ?? DateTime.Today,
                    EndDate = editForm?.EndDate,
                    Budget = editForm?.Budget ?? 0m,
                    StatusTypeId = editForm?.StatusTypeId ?? 1,
                    Statuses = statuses
                                     .Select(s => new SelectListItem(s.Name, s.Id.ToString()))
                }
            };
        }
    }
}
