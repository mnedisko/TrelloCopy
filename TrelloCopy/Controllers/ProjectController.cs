using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrelloCopy.Models;

namespace TrelloCopy.Controllers
{
    [Authorize]
    public class ProjectController : Controller
    {
        private readonly UserDbContext _UserDbContext;
        private readonly IUserAuthorityService _userAuthorityService;
        public ProjectController(UserDbContext userDbContext, IUserAuthorityService userAuthorityService)
        {
            _UserDbContext = userDbContext;
            _userAuthorityService = userAuthorityService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult CreateProject()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateProject(ProjectDataViewModel projectData)
        {
            var currentUser = _UserDbContext.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            if (currentUser != null)
            {
                var Project = new Project
                {
                    CreatedByUser = currentUser,
                    CreatedByUserId = currentUser.UserId,
                    ProjectName = projectData.ProjectName,
                    ProjectTitle = projectData.ProjectTitle,
                    CreatedAt = DateTime.Now,



                };

                _UserDbContext.Projects.Add(Project);
                _UserDbContext.SaveChanges();
                var ProjectUser = new ProjectUser
                {
                    ProjectId = Project.ProjectId,
                    UserId = Project.CreatedByUserId,
                    RoleName = "Admin",
                    AssignedAt = DateTime.Now
                };
                _UserDbContext.projectUsers.Add(ProjectUser);
                _UserDbContext.SaveChanges();
                return RedirectToAction("Details", new { id = Project.ProjectId });
            }
            return NotFound();
        }
        [HttpPost]
        public IActionResult DeleteProject(int projectId)
        {

            var project = _UserDbContext.Projects.Include(u => u.projectUsers).Include(p => p.Tasks).FirstOrDefault(p => p.ProjectId == projectId);
            if (project == null)
            {
                return NotFound();
            }
            if (project.Tasks != null && project.Tasks.Any())
            {
                _UserDbContext.Tasks.RemoveRange(project.Tasks);
            }
            if (project.projectUsers != null && project.projectUsers.Any())
            {
                _UserDbContext.projectUsers.RemoveRange(project.projectUsers);
            }

            _UserDbContext.Projects.Remove(project);
            _UserDbContext.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
        public IActionResult UpdateProject()
        {
            return Ok("");
        }
        [Authorize()]
        public IActionResult Details(int id)
        {
            var details = _UserDbContext.Projects.Include(p => p.projectUsers).ThenInclude(p => p.User).Include(p => p.CreatedByUser).Include(p => p.Tasks).FirstOrDefault(p => p.ProjectId == id);
            if (details == null)
            {
                return NotFound();
            }
            var projectMembersIds = details.projectUsers.Select(pu => pu.UserId).ToList();
            var currentUser = _UserDbContext.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);

            bool isAuthorized = details.CreatedByUserId == currentUser.UserId || details.projectUsers.Any(pu => pu.UserId == currentUser.UserId);
            if (!isAuthorized)
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            /*            ViewBag.Users = _UserDbContext.Users.Where(u => !projectMembersIds.Contains(u.UserId)).ToList();
                        ViewBag.Roles = _userAuthorityService.GetUserAuthorities();*/
            if (currentUser == null)
            {
                return NotFound();
            }
            /* ViewBag.UserRole = details.CreatedByUserId == currentUser.UserId ? "Admin" : details.projectUsers.Where(pu => pu.UserId == currentUser.UserId).Select(pu => pu.RoleName).FirstOrDefault();*/
            var viewModel = new ProjectDetailsViewModel
            {
                Project = details,
                UsersInfo = new UsersInfo
                {
                    AvailableUsers = _UserDbContext.Users.Where(u => !projectMembersIds.Contains(u.UserId)).ToList(),
                    CurrentUserRole = details.CreatedByUserId == currentUser.UserId
                ? "Admin"
                : details.projectUsers
                    .Where(pu => pu.UserId == currentUser.UserId)
                    .Select(pu => pu.RoleName)
                    .FirstOrDefault() ?? "None",
                    Roles = _userAuthorityService.GetUserAuthorities()
                },
                TaskInfo = details.Tasks.Select(t => new TaskInfo
                {
                    TaskId = t.TaskId,
                    TaskTitle = t.TaskTitle,
                    AssignedToUserName = t.AssignedToUser.UserName,
                    TaskStatus = t.TaskStatus,
                    TaskContent=t.TaskDescription,
                    CreatedAt=t.CreatedAt
                    
                }).ToList() // Tasks’ı modele ekliyoruz
            };

            return View(viewModel);
        }
        public IActionResult AddUserToProject(int UserId, int currentProjectId, string RoleName)
        {
            var currentUser = _UserDbContext.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            /*var project = _UserDbContext.Projects.FirstOrDefault(p => p.CreatedByUserId == currentUser.UserId);*/
            var addingUser = _UserDbContext.Users.FirstOrDefault(u => u.UserId == UserId);
            var project = _UserDbContext.Projects.FirstOrDefault(p => p.ProjectId == currentProjectId);
            if (currentUser == null)
            {
                return NotFound();
            }

            if (addingUser != null && project != null)
            {

                ProjectUser projectUser = new ProjectUser()
                {
                    ProjectId = currentProjectId,
                    UserId = UserId,
                    RoleName = RoleName,
                    AssignedAt = DateTime.Now,

                };
                _UserDbContext.projectUsers.Add(projectUser);
                _UserDbContext.SaveChanges();
                return RedirectToAction("Details", new { id = currentProjectId });
            }
            return NotFound();

        }


    }

}
