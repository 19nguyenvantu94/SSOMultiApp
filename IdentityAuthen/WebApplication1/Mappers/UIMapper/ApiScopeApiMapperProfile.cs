﻿// Copyright (c) Jan Škoruba. All Rights Reserved.
// Licensed under the Apache License, Version 2.0.

using AutoMapper;
using AuthenApi.Dtos.ApiScopes;
using AuthenApi.Dtos.Configuration;

namespace AuthenApi.UI.Api.Mappers
{
    public class ApiScopeApiMapperProfile : Profile
    {
        public ApiScopeApiMapperProfile()
        {
            // Api Scopes
            CreateMap<ApiScopesDto, ApiScopesApiDto>(MemberList.Destination)
                .ReverseMap();

            CreateMap<ApiScopeDto, ApiScopeApiDto>(MemberList.Destination)
                .ReverseMap();

            // Api Scope Properties
            CreateMap<ApiScopePropertiesDto, ApiScopePropertiesApiDto>(MemberList.Destination)
                .ReverseMap();

            CreateMap<ApiScopePropertyDto, ApiScopePropertyApiDto>(MemberList.Destination)
                .ReverseMap();

            CreateMap<ApiScopePropertiesDto, ApiScopePropertyApiDto>(MemberList.Destination)
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ApiScopePropertyId))
                .ReverseMap();
        }
    }
}