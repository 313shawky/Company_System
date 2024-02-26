using System.ComponentModel.DataAnnotations;

namespace CompanyG01.PL.ViewModels
{
	public class ResetPasswordViewModel
	{
		[Required(ErrorMessage = "New Password Is Required!!")]
		[DataType(DataType.Password)]
		public string NewPassword { get; set; }

		[Required(ErrorMessage = "New Password Is Required!!")]
		[DataType(DataType.Password)]
		[Compare("NewPassword", ErrorMessage = "Password Doesn't Match")]
		public string ConfirmNewPassword { get; set;}
	}
}
