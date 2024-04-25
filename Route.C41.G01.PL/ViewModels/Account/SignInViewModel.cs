using System.ComponentModel.DataAnnotations;

namespace Route.C41.G01.PL.ViewModels.Account
{
	public class SignInViewModel
	{
		[Required(ErrorMessage = "Email Is Required")]
		[EmailAddress(ErrorMessage = "Invalid Email")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Password Is Required")]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		public bool RememberMe { get; set; }
	}
}
