using System.ComponentModel.DataAnnotations;

namespace Route.C41.G01.PL.ViewModels.Account
{
	public class ResetPasswordViewModel
	{
		[Required(ErrorMessage = "Password Is Required")]
		[MinLength(5 , ErrorMessage ="Minimum length is 5 digits")]
		[DataType(DataType.Password)]
		public string NewPassword { get; set; }

		[Required(ErrorMessage = "U must confirm password !")]
		[MinLength(5, ErrorMessage = "Minimum length is 5 digits")]
		[DataType(DataType.Password)]
		[Compare(nameof(NewPassword), ErrorMessage = "confirm password dosen't match the password")]
		public string ConfirmPassword { get; set; }
		
	}
}
