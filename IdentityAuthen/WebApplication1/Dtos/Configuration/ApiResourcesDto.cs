﻿// Copyright (c) Jan Škoruba. All Rights Reserved.
// Licensed under the Apache License, Version 2.0.

using System.Collections.Generic;

namespace AuthenApi.Dtos.Configuration
{
	public class ApiResourcesDto
	{
		public ApiResourcesDto()
		{
			ApiResources = new List<ApiResourceDto>();
		}

		public int PageSize { get; set; }

		public int TotalCount { get; set; }

		public List<ApiResourceDto> ApiResources { get; set; }
	}
}