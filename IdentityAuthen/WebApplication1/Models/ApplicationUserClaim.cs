﻿using Finbuckle.MultiTenant;
using Microsoft.AspNetCore.Identity;
using Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Authen.Users.Models
{
    [MultiTenant]
    [Permissions(Actions.CRUD)]
    public class ApplicationUserClaim : IdentityUserClaim<Guid>, ISoftDelete
    {
        public override Guid UserId { get; set; }

        public string? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}
