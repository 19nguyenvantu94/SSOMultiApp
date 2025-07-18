﻿// Copyright (c) Jan Škoruba. All Rights Reserved.
// Licensed under the Apache License, Version 2.0.

namespace AuthenApi.Dtos.Identity.Interfaces
{
    public interface IUserProviderDto : IBaseUserProviderDto
    {
        string UserName { get; set; }
        string ProviderKey { get; set; }
        string LoginProvider { get; set; }
        string ProviderDisplayName { get; set; }
    }
}
