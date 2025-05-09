using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TrelloCopy.Models;
using TrelloCopy.Services;

namespace TrelloCopy.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserDbContext _dbContext;
        private readonly PasswordService _passwordService;
        public AccountController(UserDbContext dbContext,PasswordService passwordService)
        {
            _dbContext = dbContext;
            _passwordService = passwordService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel userModel)
        {
           
            if (ModelState.IsValid)
            {
                var user = _dbContext.Users.FirstOrDefault(u => u.UserName == userModel.UserName);
                if (user != null)
                {
                    bool isPasswordValid = _passwordService.VerifyPassword(user.PasswordHash, userModel.Password);
                    if (isPasswordValid)
                    {
                        /*string userRole = !string.IsNullOrEmpty(user.Role) ? user.Role : "User";*/
                        var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name,user.UserName),
                        new Claim(ClaimTypes.Role,user.Authority)
                    };
                        var ClaimsIdenity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var authProperties = new AuthenticationProperties
                        {
                            IsPersistent = true,
                            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
                        };
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(ClaimsIdenity), authProperties);
                        return RedirectToAction("Index", "Home");

                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Şifre Yanlış";
                        return View();
                    }

                }
                else
                {
                    ViewBag.ErrorMessage = "Lütfen tüm alanları doldurun.";
                    return View("Login");
                }

            }
           
            ViewBag.ErrorMessage = "Kullanıcı Adı veya Şifre Yanlış";
            return View("Login");

        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");

        }
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [HttpPost]
        public IActionResult Register(UserDataViewModel userModel)
        {      
            bool userNameControl = _dbContext.Users.Any(u => u.UserName == userModel.UserName);
            bool userMailControl = _dbContext.Users.Any(u => u.Email == userModel.Email);
            if (userNameControl)
            {
                return BadRequest("Kullanıcı adı zaten kayıtlı");
            }
            if (userMailControl)
            {
                return BadRequest("Bu email zaten kayıtlı");
            }
            if (!ModelState.IsValid)
            {
                return View(userModel);
            }
            User user = new User
            {
                Name = userModel.Name,
                UserName=userModel.UserName,
                Email = userModel.Email,
                Authority="User",
                PasswordHash = _passwordService.HashPassword(userModel.Password),
                CreatedAt = DateTime.Now

            };
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
            return RedirectToAction("Login");
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
