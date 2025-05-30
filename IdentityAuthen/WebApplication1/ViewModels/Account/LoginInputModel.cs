// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

// Original file: https://github.com/DuendeSoftware/IdentityServer.Quickstart.UI
// Modified by Jan Škoruba

using System.ComponentModel.DataAnnotations;

namespace Authen.ViewModels
{
    public class LoginInputModel
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
        public bool RememberLogin { get; set; }
        public string? ReturnUrl { get; set; }
    }
}