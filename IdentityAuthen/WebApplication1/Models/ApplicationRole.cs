﻿using BlazorIdentity.Users;
using Finbuckle.MultiTenant;
using Microsoft.AspNetCore.Identity;
using Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Authen.Users.Models
{
    [MultiTenant]
    [Permissions(Actions.CRUD)]
    public class ApplicationRole : IdentityRole<Guid>, ISoftDelete
    {

        public override Guid Id { get => base.Id; set => base.Id = value; }
        public override string Name { get => base.Name; set => base.Name = value; }
        public override string NormalizedName { get => base.NormalizedName; set => base.NormalizedName = value; }
        public override string ConcurrencyStamp { get => base.ConcurrencyStamp; set => base.ConcurrencyStamp = value; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public bool IsDeleted { get; set; } = false;
        public virtual ICollection<ApplicationRoleClaim> RoleClaims { get; set; }
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }

        public ApplicationRole() : base()
        {
            RoleClaims = new HashSet<ApplicationRoleClaim>();
        }

        public ApplicationRole(string roleName, string roleDescription = null) : base(roleName)
        {
            RoleClaims = new HashSet<ApplicationRoleClaim>();
            Description = roleDescription;
        }
    }
}