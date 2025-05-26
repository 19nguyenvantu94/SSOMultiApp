using System.ComponentModel.DataAnnotations;

namespace Authen.Users.Models.ManageViewModels
{
    public record VerifyPhoneNumberViewModel
    {
        [Required]
        public string Code { get; init; }

        [Required]
        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; init; }
    }
}
