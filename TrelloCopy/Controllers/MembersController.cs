using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrelloCopy.Models;

namespace TrelloCopy.Controllers
{
    public class MembersController : Controller
    {
        private readonly UserDbContext _userDbContext;
        private readonly IUserAuthorityService _userAuthorityService;
       /* private readonly UserAuthorityService _userAuthorityService;*/
        public MembersController(UserDbContext userDbContext , IUserAuthorityService userAuthorityService)
        {
            _userDbContext = userDbContext;
            _userAuthorityService = userAuthorityService;
        }

        public IActionResult Index()
        {
            return View();
        }
        [Authorize(Policy ="ValidRolePolicy")]
        [HttpPost]
        public IActionResult UpdateMember(int userId,int projectId,string newRole)
        {
            var member = _userDbContext.projectUsers.Where(p => p.UserId == userId).FirstOrDefault(p=>p.ProjectId==projectId);
            if (member == null)
            {
                return NotFound();
            }
            if (member.RoleName == "Admin")
            {
                return Ok("Bu kullanıcı admin");
            }
            var validRoles = _userAuthorityService.GetUserAuthorities();
            if (!validRoles.Contains(newRole))
            {
                TempData["Error"] = "Invalid Role Selected";
                return RedirectToAction("Details",new { id = projectId });
            }
            member.RoleName = newRole;
            _userDbContext.SaveChanges();


            

            return RedirectToAction("Details","Project", new { id = projectId });

        }
        [HttpPost]
        public IActionResult DeleteMember(int projectId,int userId)
        {
            var member = _userDbContext.projectUsers.Where(p => p.UserId == userId).FirstOrDefault(p => p.ProjectId == projectId);
            if (member == null)
            {
                return NotFound();

            }
            _userDbContext.projectUsers.Remove(member);
            _userDbContext.SaveChanges();

            return RedirectToAction("Details", "Project", new { id = projectId });
        }
    }
}
