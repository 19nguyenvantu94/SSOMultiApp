using System.ComponentModel.DataAnnotations;

namespace Authen.ViewModels
{
    public class LoginWithRecoveryCodeViewModel
    {
        [Required]
        [DataType(DataType.Text)]
        public string? RecoveryCode { get; set; }

        public string? ReturnUrl { get; set; }
    }
}
