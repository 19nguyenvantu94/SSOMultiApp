﻿// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

// Original file: https://github.com/DuendeSoftware/IdentityServer.Quickstart.UI
// Modified by Jan Škoruba

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Duende.IdentityServer.Events;
using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Stores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Authen.Helpers;
using Authen.ViewModels;
using Authen.Users.Constants;
using AuthenApi.Dtos.Grant;
using AuthenApi.ExceptionHandling;
using AuthenApi.Helpers;
using Microsoft.Extensions.Localization;
using AuthenApi.Services.Interfaces;
using AuthenApi.Dtos.PersistedGrants;
using AuthenApi.UI.Api.Mappers;

namespace Authen.Controllers
{
    /// <summary>
    /// This sample controller allows a user to revoke grants given to clients
    /// </summary>
    [Authorize(Policy = AuthorizationConsts.AdministrationPolicy)]
    [TypeFilter(typeof(ControllerExceptionFilterAttribute))]
    public class GrantController : BaseController
    {
        private readonly IPersistedGrantAspNetIdentityService _persistedGrantService;
        private readonly IStringLocalizer<GrantController> _localizer;

        public GrantController(IPersistedGrantAspNetIdentityService persistedGrantService,
            ILogger<GrantController> logger,
            IStringLocalizer<GrantController> localizer) : base(logger)
        {
            _persistedGrantService = persistedGrantService;
            _localizer = localizer;
        }

        [HttpGet]
        public async Task<IActionResult> PersistedGrants(int? page, string search)
        {
            ViewBag.Search = search;
            var persistedGrants = await _persistedGrantService.GetPersistedGrantsByUsersAsync(search, page ?? 1);

            return View(persistedGrants);
        }

        [HttpGet]
        public async Task<IActionResult> PersistedGrantDelete(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var grant = await _persistedGrantService.GetPersistedGrantAsync(UrlHelpers.QueryStringUnSafeHash(id));
            if (grant == null) return NotFound();

            return View(grant);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PersistedGrantDelete(PersistedGrantDto grant)
        {
            await _persistedGrantService.DeletePersistedGrantAsync(grant.Key);

            SuccessNotification(_localizer["SuccessPersistedGrantDelete"], _localizer["SuccessTitle"]);

            return RedirectToAction(nameof(PersistedGrants));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PersistedGrantsDelete(PersistedGrantsDto grants)
        {
            await _persistedGrantService.DeletePersistedGrantsAsync(grants.SubjectId);

            SuccessNotification(_localizer["SuccessPersistedGrantsDelete"], _localizer["SuccessTitle"]);

            return RedirectToAction(nameof(PersistedGrants));
        }


        [HttpGet]
        public async Task<IActionResult> PersistedGrant(string id, int? page)
        {
            var persistedGrants = await _persistedGrantService.GetPersistedGrantsByUserAsync(id, page ?? 1);
            persistedGrants.SubjectId = id;

            return View(persistedGrants);
        }
    }
}