﻿// Copyright (c) Jan Škoruba. All Rights Reserved.
// Licensed under the Apache License, Version 2.0.

namespace Authen.Helpers
{
    public class NotificationHelpers
    {
        public const string NotificationKey = "IdentityServerAdmin.Notification";

        public class Alert
        {
            public AlertType Type { get; set; }
            public string Message { get; set; }
            public string Title { get; set; }
        }

        public enum AlertType
        {
            Info,
            Success,
            Warning,
            Error
        }
    }
}