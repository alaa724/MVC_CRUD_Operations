using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Route.C41.G01.DAL.Models;
using Route.C41.G01.PL.Services.EmailSender;
using Route.C41.G01.PL.ViewModels.Account;
using System.Threading.Tasks;

namespace Route.C41.G01.PL.Controllers
{
    public class AccountController : Controller
    {
		private readonly IEmailSender _emailSender;
		private readonly IConfiguration _configuration;
		private readonly UserManager<ApplicationUsers> _userManager;
		private readonly SignInManager<ApplicationUsers> _signInManager;

		public AccountController(
            IEmailSender emailSender,
            IConfiguration configuration,
            UserManager<ApplicationUsers> userManager ,
            SignInManager<ApplicationUsers> signInManager)
		{
			_emailSender = emailSender;
			_configuration = configuration;
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

        #region SignOut

        public async new Task<ActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(SignIn));
        }

        #endregion

        #region Forget Password
        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendResetPasswordEmail(ForgetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user is not null)
                {
                    var resetPasswordToken = await _userManager.GeneratePasswordResetTokenAsync(user);

                    var resetPasswordUrl = Url.Action("ResetPassword", "Account", new { email = user.Email, token = resetPasswordToken }, "https", "localhost:5001");

                    await _emailSender.SendAsync(
                        from: _configuration["EmailSettings:SmtpClientServer"],
                        recipents: model.Email,
                        subject: "Reset Your Password",
                        body: resetPasswordUrl);
                    return RedirectToAction(nameof(CheckYourEmail));
                }
                ModelState.AddModelError(string.Empty, "There Is No Account With This Email");
            }
            return View(model);
        }

        public IActionResult CheckYourEmail()
        {
            return View();
        }
        #endregion

        #region Reset Password


        public IActionResult ResetPassword(string email , string token)
        {
            TempData["Email"] = email;
            TempData["Token"] = token;
            return View();
        }

        [HttpPost]
         public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var email = TempData["Email"] as string;
                var token = TempData["Token"] as string;

                var user = await _userManager.FindByEmailAsync(email);

                if(user is not null)
                {
                    await _userManager.ResetPasswordAsync(user, token, model.NewPassword);

                    return RedirectToAction(nameof(SignIn));

                }
                ModelState.AddModelError(string.Empty, "Url is not valid");
            }
            return View(model);
        }

        

        #endregion

    }
}
