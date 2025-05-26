// Copyright (c) Jan Škoruba. All Rights Reserved.
// Licensed under the Apache License, Version 2.0.

namespace AuthenApi.Dtos.Identity.Interfaces
{
    public interface IRoleDto : IBaseRoleDto
    {
        string Name { get; set; }
    }
}
