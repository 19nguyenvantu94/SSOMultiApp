﻿// Copyright (c) Jan Škoruba. All Rights Reserved.
// Licensed under the Apache License, Version 2.0.

using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AuthenApi.Helpers
{
    public static class DbContextHelpers
    {
        /// <summary>
        /// Get the table name of an entity in the given DbContext
        /// </summary>
        /// <typeparam name="TDbContext"></typeparam>
        /// <param name="serviceProvider"></param>
        /// <param name="entityTypeName">If specified, the full name of the type of the entity. 
        /// Otherwise, the first entity in the DbContext will be retrieved</param>
        /// <returns></returns>
        public static string GetEntityTable<TDbContext>(IServiceProvider serviceProvider, string entityTypeName = null)
            where TDbContext : DbContext
        {
            var db = serviceProvider.GetService<TDbContext>();
            if (db != null)
            {
                var entityType = entityTypeName != null ? db.Model.FindEntityType(entityTypeName) : db.Model.GetEntityTypes().FirstOrDefault();
                if (entityType != null)
                    return entityType.GetTableName();
            }

            return null;
        }
    }
}
