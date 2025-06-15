// Copyright (c) Jan Škoruba. All Rights Reserved.
// Licensed under the Apache License, Version 2.0.

using Authen.Helpers;
using Authen.Models;
using AuthenApi.Common;
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
}