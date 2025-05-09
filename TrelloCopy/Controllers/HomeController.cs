using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrelloCopy.Models;

namespace TrelloCopy.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly UserDbContext _userDbContext;

    public HomeController(ILogger<HomeController> logger,UserDbContext userDbContext)
    {
        _logger = logger;
        _userDbContext = userDbContext;
    }

    public IActionResult AccessDenied()
    {
        return View();
    }
    public IActionResult Index()
{
    var user = _userDbContext.Users
        .FirstOrDefault(u => u.UserName == User.Identity.Name);

    if (user == null)
    {
        return View();
    }
    

    var viewModel = new UserProjectViewModel
    {
        UserName = user.UserName,
        OwnedProject = _userDbContext.Projects
            .Where(p => p.CreatedByUserId == user.UserId)
            .Select(p => new ProjectInfo
            {
                ProjectId = p.ProjectId,
                ProjectName = p.ProjectName,
                CreatedByUserId = p.CreatedByUserId,
                CreatedByUserName = p.CreatedByUser.UserName,
                CreatedAt = p.CreatedAt,
                UserRole="Owner"
            })
            .ToList(),
        AssignedProject = _userDbContext.projectUsers
            .Where(pu => pu.UserId == user.UserId && pu.Project.CreatedByUserId != user.UserId)
            .Select(pu => new ProjectInfo
            {
                ProjectId = pu.Project.ProjectId,
                ProjectName = pu.Project.ProjectName,
                CreatedByUserId = pu.Project.CreatedByUserId,
                CreatedByUserName = pu.Project.CreatedByUser.UserName,
                CreatedAt = pu.Project.CreatedAt,
                UserRole = pu.RoleName

            })
            .Distinct()
            .ToList()
    };

    return View("Index", viewModel);
    }


    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    public IActionResult Details()
    {
        var user = _userDbContext.Users.Include(u=>u.Projects).Include(u=>u.projectUsers).FirstOrDefault(u => u.UserName == User.Identity.Name);
        if (user == null)
        {
            return NotFound();
        }
        /*var projects = _userDbContext.Projects.Include(p => p.ProjectId).Where(p => p.CreatedByUserId == user.UserId).ToList();*/
        return View("Details",user);
    }
}
