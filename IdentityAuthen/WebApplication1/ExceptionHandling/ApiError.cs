﻿// Copyright (c) Jan Škoruba. All Rights Reserved.
// Licensed under the Apache License, Version 2.0.

namespace AuthenApi.ExceptionHandling
{
    public class ApiError
    {
        public string Code { get; set; }

        public string Description { get; set; }
    }
}