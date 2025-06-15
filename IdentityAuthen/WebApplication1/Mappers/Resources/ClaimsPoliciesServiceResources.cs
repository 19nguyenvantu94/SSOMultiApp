// Copyright (c) Jan Škoruba. All Rights Reserved.
// Licensed under the Apache License, Version 2.0.

using AuthenApi.Helpers;

namespace AuthenApi.Resources
{
    public class ClaimsPoliciesServiceResources : IClaimsPoliciesServiceResources
    {
         
        public virtual ResourceMessage ClaimsPoliciesDoesNotExist()
        {
            return new ResourceMessage()
            {
                Code = nameof(ClaimsPoliciesDoesNotExist),
                Description = "ClaimsPoliciesDoesNotExist"
            };
        }

        public virtual ResourceMessage ClaimsPoliciesExistsKey()
        {
            return new ResourceMessage()
            {
                Code = nameof(ClaimsPoliciesExistsKey),
                Description = "ClaimsPoliciesExistsKey"
            };
        }

        public virtual ResourceMessage ClaimsPoliciesExistsValue()
        {
            return new ResourceMessage() { Code = nameof(ClaimsPoliciesDoesNotExist), Description = "ClaimsPoliciesDoesNotExist" };
        }

        public virtual ResourceMessage ClaimsPoliciesPropertyDoesNotExist()
        {
            return new ResourceMessage() { Code = nameof(ClaimsPoliciesDoesNotExist), Description = "ClaimsPoliciesDoesNotExist" };
        }

        public virtual ResourceMessage ClaimsPoliciesPropertyExistsKey()
        {
            return new ResourceMessage() { Code = nameof(ClaimsPoliciesDoesNotExist), Description = "ClaimsPoliciesDoesNotExist" };
        }

        public virtual ResourceMessage ClaimsPoliciesPropertyExistsValue()
        {
            return new ResourceMessage() { Code = nameof(ClaimsPoliciesDoesNotExist), Description = "ClaimsPoliciesDoesNotExist" };
        }
    }
}
