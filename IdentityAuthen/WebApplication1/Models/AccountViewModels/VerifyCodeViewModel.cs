﻿using System.ComponentModel.DataAnnotations;

namespace Authen.Users.Models.AccountViewModels
{
    public record VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; init; }

        [Required]
        public string Code { get; init; }

        public string ReturnUrl { get; init; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; init; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; init; }
    }
}
