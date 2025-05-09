using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using TrelloCopy.Models;
using TrelloCopy.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<ProjectDetailsViewModel>();
builder.Services.AddScoped<PasswordService>();
builder.Services.AddSingleton<IUserAuthorityService, UserAuthorityService>();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ValidRolePolicy", policy =>
        policy.RequireAssertion(context =>
        {
            var httpContext = context.Resource as HttpContext;
            var newRole = httpContext?.Request.Form["newRole"].FirstOrDefault();
            var userAuthorityService = httpContext?.RequestServices.GetRequiredService<IUserAuthorityService>();
            var validRoles = userAuthorityService?.GetUserAuthorities();

            return newRole != null && validRoles != null && validRoles.Contains(newRole);
        }));
});
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options => { options.LoginPath = "/Account/Login"; });
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
