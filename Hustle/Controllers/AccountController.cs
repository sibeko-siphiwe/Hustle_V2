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
        private const string WorkerRole = "Worker";
        private const string EmployerRole = "Employer";

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            byte[] profileImageBinary = null;
             
            if (model.ImageUrl != null && model.ImageUrl.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    await model.ImageUrl.CopyToAsync(ms);
                    profileImageBinary = ms.ToArray();
                }
            }

            var user = new ApplicationUser
            {
                UserName = model.EmailAddress,
                Email = model.EmailAddress,
                FirstName = model.FirstName,
                LastName = model.LastName,
                ProfileImage = profileImageBinary,  
                PhoneNumber = model.PhoneNumber,
                Location = model.Location,
                Bio = model.Bio,
            };

            var createResult = await _userManager.CreateAsync(user, model.Password);

            if (createResult.Succeeded)
            { 
                string assignedRole = model.Role ? WorkerRole : EmployerRole;

                var roleResult = await _userManager.AddToRoleAsync(user, assignedRole);

                if (roleResult.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Profile", "Account");
                }
                 
                foreach (var error in roleResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                 
            }
            else
            { 
                foreach (var error in createResult.Errors)
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
