// Copyright (c) Jan Škoruba. All Rights Reserved.
// Licensed under the Apache License, Version 2.0.

using Authen.Helpers;
using Authen.Models;
using AuthenApi.Common;
using Duende.IdentityServer.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace AuthenApi.Dtos.Grant
{
    public class ClientsPoliciesDto : Pager
    {
        public ClientsPoliciesDto()
        {
            ClientsPolicies = new List<ClientClaimPolicy>();
        }

        public int TotalCount { get; set; }

        public int PageSize { get; set; }
        public List<ClientClaimPolicy> ClientsPolicies { get; set; }
    }

 


    public class ClientsIdDto
    {

        public int Id { get; set; }

        public Client? Client { get; set; }

        public List<SelectItem>? Roles { get; set; }


        public List<SelectItem>? ClientsList { get; set; }

        public List<SelectItem>? RolesList { get; set; }


        public int ClientId { get; set; }

        public Guid RoleId { get; set; }

    }

}