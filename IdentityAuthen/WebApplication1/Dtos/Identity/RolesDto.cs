﻿// Copyright (c) Jan Škoruba. All Rights Reserved.
// Licensed under the Apache License, Version 2.0.

using System.Collections.Generic;
using System.Linq;
using Authen.Dtos.Identity;
using AuthenApi.Dtos.Identity.Interfaces;

namespace AuthenApi.Dtos.Identity
{
    public class RolesDto<TRoleDto, TKey>: IRolesDto where TRoleDto : RoleDto<TKey>
    {
        public RolesDto()
        {
            Roles = new List<TRoleDto>();
        }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public List<TRoleDto> Roles { get; set; }

        List<IRoleDto> IRolesDto.Roles => Roles.Cast<IRoleDto>().ToList();
    }
}