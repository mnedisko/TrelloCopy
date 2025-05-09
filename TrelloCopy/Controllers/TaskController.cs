using Microsoft.AspNetCore.Mvc;
using TrelloCopy.Models;

namespace TrelloCopy.Controllers
{
    public class TaskController : Controller
    {
        private readonly UserDbContext _userDbContext;
        public TaskController(UserDbContext userDbContext)
        {
            _userDbContext = userDbContext;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult CreateTask(string taskTitle, string taskContent, int userId, int currentProjectId)
        {
            var member = _userDbContext.projectUsers.Where(p => p.UserId == userId).FirstOrDefault(p => p.ProjectId == currentProjectId);
            var currentUser = User.Identity.Name;
            if (member == null)
            {
                return NotFound();

            }
            var task = new Tasks()
            {
                ProjectId = currentProjectId,
                AssignedToUserId = userId,
                CreatedAt = DateTime.Now,
                TaskDescription = taskContent,
                TaskTitle = taskTitle
                



            };
            _userDbContext.Tasks.Add(task);
            _userDbContext.SaveChanges();
            return RedirectToAction("details", "Project", new { id = currentProjectId });
        }
    }

}
