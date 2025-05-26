// Copyright (c) Jan Škoruba. All Rights Reserved.
// Licensed under the Apache License, Version 2.0.

using AuthenApi.Helpers;

namespace AuthenApi.Resources
{
    public interface IIdentityProviderServiceResources
    {
        ResourceMessage IdentityProviderDoesNotExist();

        ResourceMessage IdentityProviderExistsKey();

        ResourceMessage IdentityProviderExistsValue();

    }
}
