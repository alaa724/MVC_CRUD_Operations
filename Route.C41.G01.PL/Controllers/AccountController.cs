using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Route.C41.G01.DAL.Models;
using Route.C41.G01.PL.ViewModels.User;
using System.Threading.Tasks;

namespace Route.C41.G01.PL.Controllers
{
    public class AccountController : Controller
    {
		private readonly UserManager<ApplicationUsers> _userManager;
		private readonly SignInManager<ApplicationUsers> _signInManager;

		public AccountController(UserManager<ApplicationUsers> userManager , SignInManager<ApplicationUsers> signInManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
		}


		#region Sign Up
		//[HttpGet]
		public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user =await _userManager.FindByNameAsync(model.UserName);
                if(user is null)
                {
                    user = new ApplicationUsers()
                    {
                        FName = model.FirstName,
                        LName = model.LastName,
                        UserName = model.UserName,
                        Email = model.Email,
                        IsAgree = model.IsAgree
                    };

                    var result = await _userManager.CreateAsync(user, model.Password);

                    if (result.Succeeded)
                        return RedirectToAction(nameof(SignIn));

                    foreach (var error in result.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);

                }

                ModelState.AddModelError(string.Empty, "This UserName is already in use for another account");
            }
            return View(model);
        }

        #endregion


        #region Sign In

        public IActionResult SignIn()
        {
            return View();
        }


        #endregion
    }
}
