// Copyright (c) Jan Škoruba. All Rights Reserved.
// Licensed under the Apache License, Version 2.0.


using AuthenApi.ExceptionHandling;

namespace AuthenApi.Resources
{
    public interface IApiErrorResources
    {
        ApiError CannotSetId();
    }
}