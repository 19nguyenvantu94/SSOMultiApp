﻿// Copyright (c) Jan Škoruba. All Rights Reserved.
// Licensed under the Apache License, Version 2.0.

using System.Collections.Generic;

namespace AuthenApi.Dtos.Identity.Interfaces
{
    public interface IRolesDto
    {
        int PageSize { get; set; }
        int TotalCount { get; set; }
        List<IRoleDto> Roles { get; }
    }
}
