﻿// Copyright (c) Jan Škoruba. All Rights Reserved.
// Licensed under the Apache License, Version 2.0.

using System;

namespace AuthenApi.ExceptionHandling
{
    public class UserFriendlyErrorPageException : Exception
    {
        public string ErrorKey { get; set; }
        
        public UserFriendlyErrorPageException(string message) : base(message)
        {
        }

        public UserFriendlyErrorPageException(string message, string errorKey) : base(message)
        {
            ErrorKey = errorKey;
        }

        public UserFriendlyErrorPageException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
