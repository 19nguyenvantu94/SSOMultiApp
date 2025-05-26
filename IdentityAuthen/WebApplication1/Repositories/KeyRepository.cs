// Copyright (c) Jan Škoruba. All Rights Reserved.
// Licensed under the Apache License, Version 2.0.

using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Duende.IdentityServer.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using AuthenApi.Repositories.Interfaces;
using Authen.Data;
using AuthenApi.Common;
using AuthenApi.Extensions;
using AuthenApi.Dtos.Enums;

namespace AuthenApi.Repositories
{
    public class KeyRepository : IKeyRepository
    {
        protected readonly ApplicationDbContext DbContext;
        public bool AutoSaveChanges { get; set; } = true;

        public KeyRepository(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public virtual async Task<PagedList<Key>> GetKeysAsync(int page = 1, int pageSize = 10, CancellationToken cancellationToken = default)
        {
            var pagedList = new PagedList<Key>();

            var clients = await DbContext.Keys.PageBy(x => x.Id, page, pageSize)
                .AsNoTracking()
                .ToListAsync(cancellationToken: cancellationToken);

            pagedList.Data.AddRange(clients);

            pagedList.TotalCount = await DbContext.Keys.CountAsync(cancellationToken: cancellationToken);
            pagedList.PageSize = pageSize;

            return pagedList;
        }

        public virtual async Task<Key> GetKeyAsync(string id, CancellationToken cancellationToken = default)
        {
            return await DbContext.Keys.Where(x => x.Id == id)
                .AsNoTracking()
                .SingleOrDefaultAsync(cancellationToken: cancellationToken);
        }

        public virtual async Task DeleteKeyAsync(string id, CancellationToken cancellationToken = default)
        {
            var keyToDelete = await DbContext.Keys.Where(x => x.Id == id).SingleOrDefaultAsync(cancellationToken: cancellationToken);

            DbContext.Keys.Remove(keyToDelete);

            await AutoSaveChangesAsync(cancellationToken);
        }

        public virtual async Task<bool> ExistsKeyAsync(string id, CancellationToken cancellationToken = default)
        {
            return await DbContext.Keys.Where(x => x.Id == id).AnyAsync(cancellationToken: cancellationToken);
        }

        public virtual async Task<int> SaveAllChangesAsync(CancellationToken cancellationToken = default)
        {
            return await DbContext.SaveChangesAsync(cancellationToken);
        }

        protected virtual async Task<int> AutoSaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return AutoSaveChanges ? await DbContext.SaveChangesAsync(cancellationToken) : (int)SavedStatus.WillBeSavedExplicitly;
        }
    }
}