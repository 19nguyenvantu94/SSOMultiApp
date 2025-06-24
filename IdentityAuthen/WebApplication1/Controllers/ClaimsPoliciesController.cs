// Copyright (c) Jan Škoruba. All Rights Reserved.
// Licensed under the Apache License, Version 2.0.

using Authen.Models;
using Authen.Users.Constants;
using AuthenApi.Dtos.Configuration;
using AuthenApi.Dtos.Grant;
using AuthenApi.ExceptionHandling;
using AuthenApi.Helpers;
using AuthenApi.Services;
using AuthenApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace Authen.Controllers
{
    [Authorize(Policy = AuthorizationConsts.AdministrationPolicy)]
    [TypeFilter(typeof(ControllerExceptionFilterAttribute))]
    public class ClaimsPoliciesController : BaseController
    {
        private readonly IClaimsPoliciesService _claimPoliciesService;
        private readonly IStringLocalizer<ClaimsPoliciesController> _localizer;

        public ClaimsPoliciesController(IClaimsPoliciesService claimsPoliciesService,
            IStringLocalizer<ClaimsPoliciesController> localizer,
            ILogger<ClaimsPoliciesController> logger)
            : base(logger)
        {
            _claimPoliciesService = claimsPoliciesService;
             _localizer = localizer;
        }

        [HttpGet]
        public async Task<IActionResult> ClientPolicies(string search, int? page)
        {
          
            var claims = await _claimPoliciesService.GetClientPoliciesAsync(search, page ?? 1);

            return View(claims);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ClientPolicy(ClientsIdDto identityResource)
        {
            if (!ModelState.IsValid)
            {
                return View(identityResource);
            }

            identityResource = _claimPoliciesService.BuildIdentityResourceViewModel(identityResource);

            int identityResourceId;

            if (identityResource.Id == 0)
            {
                identityResourceId = await _claimPoliciesService.AddClaimsPolicies(identityResource);
            }
            else
            {
                identityResourceId = identityResource.Id;
                await _claimPoliciesService.UpdateClaimsPolicies(identityResource);
            }

            SuccessNotification(string.Format(_localizer["SuccessAddIdentityResource"], identityResource.ClientId), _localizer["SuccessTitle"]);

            return RedirectToAction(nameof(ClientPoliciesById), new { Id = identityResource.Id });
        }

        [HttpGet]
        public async Task<IActionResult> ClientPoliciesById(string id)
        {
            if (id.IsNotPresentedValidNumber())
            {
                return NotFound();
            }

            int.TryParse(id, out var identityResourceId);

            //if (id == default)
            //{
            //    var createValue = await _claimPoliciesService.GetClaimsPolicies(identityResourceId);
            //    return View(createValue);
            //}

         
            var identityResource = await _claimPoliciesService.GetClaimsPolicies(identityResourceId);

            return View(identityResource);
        }

        [HttpGet]
        public async Task<IActionResult> ClientPolicyDelete(int id)
        {
            if (id == 0) return NotFound();

            await _claimPoliciesService.ClientPolicyDelete(id);

            SuccessNotification(_localizer["SuccessClientPolicyDelete"], _localizer["SuccessTitle"]);

            return RedirectToAction(nameof(ClientPolicies));
        }

    }
}