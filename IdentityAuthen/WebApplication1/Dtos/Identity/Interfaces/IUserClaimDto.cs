// Copyright (c) Jan Škoruba. All Rights Reserved.
// Licensed under the Apache License, Version 2.0.

namespace AuthenApi.Dtos.Identity.Interfaces
{
    public interface IUserClaimDto : IBaseUserClaimDto
    {
        string ClaimType { get; set; }
        string ClaimValue { get; set; }
    }
}
