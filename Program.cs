using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskAssessment.Areas.Identity.Data;
using TaskAssessment.Data;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("TaskAssessmentContextConnection") ?? throw new InvalidOperationException("Connection string 'TaskAssessmentContextConnection' not found.");

Console.WriteLine(connectionString);
builder.Services.AddDbContext<TaskAssessmentContext>(options => options.UseSqlServer(connectionString), ServiceLifetime.Transient);

builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<TaskAssessmentContext>();

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();




builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignOutScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
    .AddCookie(options =>
    {
        options.Cookie.Name = "YourAppCookieName";
        options.LoginPath = "/Auth/Login"; // Specify your login path
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    });

var cookiePolicyOptions = new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Lax,
};

var app = builder.Build();
app.UseCookiePolicy(cookiePolicyOptions);

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
