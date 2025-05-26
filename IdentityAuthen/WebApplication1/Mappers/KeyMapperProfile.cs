// Copyright (c) Jan Škoruba. All Rights Reserved.
// Licensed under the Apache License, Version 2.0.

using AutoMapper;
using Duende.IdentityServer.EntityFramework.Entities;
using AuthenApi.Dtos.Grant;
using AuthenApi.Dtos.Key;
using AuthenApi.Common;

namespace AuthenApi.Mappers
{
    public class KeyMapperProfile : Profile
    {
        public KeyMapperProfile()
        {
            CreateMap<PagedList<Key>, KeysDto>(MemberList.Destination)
                .ForMember(x => x.Keys,
                    opt => opt.MapFrom(src => src.Data));

            CreateMap<Key, KeyDto>(MemberList.Destination)
                .ReverseMap();
        }
    }
}