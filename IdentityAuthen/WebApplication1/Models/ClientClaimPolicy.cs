﻿using Authen.Users;
using Authen.Users.Models;
using Duende.IdentityServer.Models;
using Finbuckle.MultiTenant;
using Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Authen.Models
{

    [MultiTenant]
    [Permissions(Actions.CRUD)]
    public class ClientClaimPolicy
    {

        [Key]
        public int Id { get; set; }


        public int IdClient { get; set; }

        public Client Client { get; set; }

        public virtual ICollection<ClientClaimPolicyRole> PolicyRoles { get; set; }
    }
}
