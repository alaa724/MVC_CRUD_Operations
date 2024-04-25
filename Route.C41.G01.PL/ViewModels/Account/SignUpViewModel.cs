using System.ComponentModel.DataAnnotations;

namespace Route.C41.G01.PL.ViewModels.Account
{
	public class SignUpViewModel
	{
		[Required(ErrorMessage ="UserName Is Required")]
		public string UserName { get; set; }

		[Required(ErrorMessage = "FirstName Is Required")]
		[Display(Name ="First Name")]
		public string FirstName { get; set; }

		[Required(ErrorMessage = "LastName Is Required")]
		[Display(Name = "Last Name")]
		public string LastName { get; set; }

		[Required(ErrorMessage ="Email Is Required")]
		[EmailAddress(ErrorMessage ="Invalid Email")]
		public string Email { get; set; }

		[Required(ErrorMessage ="Password Is Required")]
		//[MinLength(5 , ErrorMessage ="Minimum length is 5 digits")]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[Required(ErrorMessage = "U must confirm password !")]
		[MinLength(5, ErrorMessage = "Minimum length is 5 digits")]
		[DataType(DataType.Password)]
		[Compare(nameof(Password) , ErrorMessage = "confirm password dosen't match the password")]
		public string ConfirmPassword { get; set; }

		public bool IsAgree { get; set; }

	}
}
