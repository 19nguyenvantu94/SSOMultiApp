// Copyright (c) Jan Škoruba. All Rights Reserved.
// Licensed under the Apache License, Version 2.0.

using AuthenApi.Helpers;

namespace AuthenApi.Resources
{
    public interface IClaimsPoliciesServiceResources
    {
        ResourceMessage ClaimsPoliciesDoesNotExist();

        ResourceMessage ClaimsPoliciesExistsKey();

        ResourceMessage ClaimsPoliciesExistsValue();

        ResourceMessage ClaimsPoliciesPropertyDoesNotExist();

        ResourceMessage ClaimsPoliciesPropertyExistsValue();

        ResourceMessage ClaimsPoliciesPropertyExistsKey();
    }
}
