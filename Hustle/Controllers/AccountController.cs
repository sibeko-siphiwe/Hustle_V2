using Hustle.Models;
using Hustle.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Hustle.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private const string Role = "Worker";
        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                byte[] imageUrl = null;

                if (model.ImageUrl != null && model.ImageUrl.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        model.ImageUrl.CopyTo(ms);
                        imageUrl = ms.ToArray();
                    }
                }

                var user = new ApplicationUser
                {
                    UserName = model.EmailAddress,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    ProfileImage = imageUrl,
                    PhoneNumber = model.PhoneNumber,
                    Location = model.Location,
                    Bio = model.Bio,
                    Email = model.EmailAddress,
                };

                var results = await _userManager.CreateAsync(user, model.Password);

                if (results.Succeeded)
                {
                    if (await _roleManager.FindByNameAsync(Role) == null)
                    {
                        await _roleManager.CreateAsync(new IdentityRole(Role));
                    }

                    var result = await _userManager.AddToRoleAsync(user, Role);

                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);

                        return RedirectToAction("Profile", "Account");
                    }

                }
                foreach (var error in results.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        [Authorize]
        public IActionResult Profile()
        {
            return View();
        }
    }
}
