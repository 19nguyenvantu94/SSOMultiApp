﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Finbuckle.MultiTenant;

namespace Authen.Users.Models
{
    [MultiTenant]
    public partial class UserProfile
    {
        [Key]
        public Guid Id { get; set; } = new Guid();

        public Guid UserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public string LastPageVisited { get; set; } = "/";
        public bool IsNavOpen { get; set; } = true;
        public bool IsNavMinified { get; set; } = false;
        public int Count { get; set; } = 0;
        public DateTime LastUpdatedDate { get; set; } = DateTime.MinValue;
        public string Culture { get; set; } = string.Empty;

        public bool IsDarkMode { get; set; } = false;

        [Column(TypeName = "nvarchar(64)")]
        public string TenantId { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string AvatarUrl { get; set; } = string.Empty;
    }
}
