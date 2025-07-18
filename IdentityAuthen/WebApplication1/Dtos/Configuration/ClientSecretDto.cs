﻿// Copyright (c) Jan Škoruba. All Rights Reserved.
// Licensed under the Apache License, Version 2.0.

using System;
using System.ComponentModel.DataAnnotations;

namespace AuthenApi.Dtos.Configuration
{
    public class ClientSecretDto
    {
        [Required]
        public string Type { get; set; } = "SharedSecret";

		public int Id { get; set; }

		public string Description { get; set; }

        [Required]
		public string Value { get; set; }

		public DateTime? Expiration { get; set; }

        public DateTime Created { get; set; }
	}
}
