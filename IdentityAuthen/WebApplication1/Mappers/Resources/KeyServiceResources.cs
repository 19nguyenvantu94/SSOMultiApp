// Copyright (c) Jan Škoruba. All Rights Reserved.
// Licensed under the Apache License, Version 2.0.

using AuthenApi.Helpers;

namespace AuthenApi.Resources
{
    public class KeyServiceResources : IKeyServiceResources
    {
        public ResourceMessage KeyDoesNotExist()
        {
            return new ResourceMessage()
            {
                Code = nameof(KeyDoesNotExist),
                Description = "KeyDoesNotExist"
            };
        }
    }
}