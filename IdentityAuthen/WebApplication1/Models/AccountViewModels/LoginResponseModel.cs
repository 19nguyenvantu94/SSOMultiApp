﻿namespace Authen.Users.Models.AccountViewModels
{
    public class LoginResponseModel
    {
        public bool RequiresTwoFactor { get; set; }

        public string LastPageVisited { get; set; }

        public string ReturnUrl { get; set; }
    }
}
