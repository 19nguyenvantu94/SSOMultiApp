﻿// Copyright (c) Jan Škoruba. All Rights Reserved.
// Licensed under the Apache License, Version 2.0.

using AuthenApi.Helpers;

namespace AuthenApi.Resources
{
    public class IdentityProviderServiceResources : IIdentityProviderServiceResources
    {
        public virtual ResourceMessage IdentityProviderDoesNotExist()
        {
            return new ResourceMessage()
            {
                Code = nameof(IdentityProviderDoesNotExist),
                Description = "IdentityProviderDoesNotExist"
            };
        }

        public virtual ResourceMessage IdentityProviderExistsKey()
        {
            return new ResourceMessage()
            {
                Code = nameof(IdentityProviderExistsKey),
                Description = "IdentityProviderExistsKey"
            };
        }

        public virtual ResourceMessage IdentityProviderExistsValue()
        {
            return new ResourceMessage()
            {
                Code = nameof(IdentityProviderExistsValue),
                Description = "IdentityProviderExistsValue"
            };
        }
        
    }
}
