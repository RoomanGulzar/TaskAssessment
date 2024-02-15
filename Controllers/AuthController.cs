using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskAssessment.Areas.Identity.Data;
using TaskAssessment.Data;
using TaskAssessment.Models;

namespace TaskAssessment.Controllers
{
    public class AuthController : Controller
    {
        private readonly TaskAssessmentContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        public AuthController(TaskAssessmentContext context, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Login()
        {
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            foreach (var cookie in Request.Cookies.Keys)
            {
                Response.Cookies.Delete(cookie);
            }
            // Get the current user's identity
            var userIdentity = (ClaimsIdentity)User.Identity;
            // Check if the claim is in the identity
            var claimToRemove = userIdentity.FindFirst(ClaimTypes.Name);
            if (claimToRemove != null)
            {
                userIdentity.RemoveClaim(claimToRemove);
            }
            else
            {
                // If the claim is not in the identity, check the principal
                var userPrincipal = (ClaimsPrincipal)User;
                var claimInPrincipal = userPrincipal.FindFirst(ClaimTypes.Name);
                if (claimInPrincipal != null)
                {
                    // Remove the claim from the principal
                    ((ClaimsIdentity)userPrincipal.Identity).RemoveClaim(claimInPrincipal);
                }
            }





            await HttpContext.SignOutAsync(
        CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public async Task<IActionResult> Login(String Email, string Password)
        {
            var u = await _userManager.FindByEmailAsync(Email);
            if (u != null)
            {
                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Email, u.Email),
                        new Claim(ClaimTypes.Name, u.UserName),
                    };

                var claimsIdentity = new ClaimsIdentity(
                             claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true
                };
                await HttpContext.SignInAsync(
                                     CookieAuthenticationDefaults.AuthenticationScheme,
                                     new ClaimsPrincipal(claimsIdentity),
                                     authProperties);
                await _signInManager.SignInWithClaimsAsync(u, null, claims);

                var s=_signInManager.IsSignedIn(User);
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(UserVm user)
        {
            User u = new()
            {

                UserName = user.Name,
                Email = user.Email,
                EmailConfirmed = true

            };

            var res = await _userManager.CreateAsync(u, user.Password);
            if (res.Succeeded)
            {
                await _signInManager.SignInAsync(u, false);
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
    }
}
