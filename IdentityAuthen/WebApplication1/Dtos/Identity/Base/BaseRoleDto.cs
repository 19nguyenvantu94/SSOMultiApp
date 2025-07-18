﻿// Copyright (c) Jan Škoruba. All Rights Reserved.
// Licensed under the Apache License, Version 2.0.

using System.Collections.Generic;
using AuthenApi.Dtos.Identity.Interfaces;

namespace AuthenApi.Dtos.Identity.Base
{
    public class BaseRoleDto<TRoleId> : IBaseRoleDto
    {
        public TRoleId Id { get; set; }

        public bool IsDefaultId() => EqualityComparer<TRoleId>.Default.Equals(Id, default(TRoleId));

        object IBaseRoleDto.Id => Id;
    }
}