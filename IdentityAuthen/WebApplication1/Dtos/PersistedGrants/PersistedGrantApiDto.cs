// Copyright (c) Jan Škoruba. All Rights Reserved.
// Licensed under the Apache License, Version 2.0.

using System;

namespace AuthenApi.Dtos.PersistedGrants
{
    public class PersistedGrantApiDto
    {
        public long Id { get; set; }
        public string Key { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string SubjectId { get; set; } = string.Empty;
        public string SubjectName { get; set; } = string.Empty;
        public string ClientId { get; set; } = string.Empty;
        public DateTime CreationTime { get; set; }  
        public DateTime? Expiration { get; set; }
        public string Data { get; set; } = string.Empty;
        public DateTime? ConsumedTime { get; set; }
        public string SessionId { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}