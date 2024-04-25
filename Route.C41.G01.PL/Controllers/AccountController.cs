using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Route.C41.G01.DAL.Models;
using Route.C41.G01.PL.ViewModels.Account;
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

        [HttpPost]
		public async Task<IActionResult> SignIn(SignInViewModel model)
		{
            if(ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if(user is not null)
                {
                    var flage = await _userManager.CheckPasswordAsync(user, model.Password);
                    if (flage)
                    {
                        var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                        if(result.IsLockedOut)
                            ModelState.AddModelError(string.Empty, "Your Account Is Not Locked!!");

                        if (result.Succeeded)
                            return RedirectToAction(nameof(HomeController.Index) , "Home");

                        if (result.IsNotAllowed)
                            ModelState.AddModelError(string.Empty, "Your Account Is Not Confirmed Yet!!");

					}
                }
                ModelState.AddModelError(string.Empty, "Invalid Login");
            }
			return View(model);
		}


		#endregion
	}
}
