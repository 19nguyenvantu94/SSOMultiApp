// Copyright (c) Jan Škoruba. All Rights Reserved.
// Licensed under the Apache License, Version 2.0.

using AuthenApi.Dtos.Identity.Interfaces;

namespace AuthenApi.Dtos.Identity.Base
{
    public class BaseUserClaimDto<TUserId> : IBaseUserClaimDto
    {
        public Guid ClaimId { get; set; }

        public TUserId UserId { get; set; }

        object IBaseUserClaimDto.UserId => UserId;
    }
}