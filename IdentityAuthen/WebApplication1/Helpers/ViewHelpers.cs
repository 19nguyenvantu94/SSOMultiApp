﻿// Copyright (c) Jan Škoruba. All Rights Reserved.
// Licensed under the Apache License, Version 2.0.

namespace AuthenApi.Helpers
{
    public static class ViewHelpers
    {
        public static string GetClientName(string clientId, string clientName)
        {
            return $"{clientName} ({clientId})";
        }
    }
}
