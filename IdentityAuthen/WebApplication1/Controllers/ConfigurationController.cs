﻿// Copyright (c) Jan Škoruba. All Rights Reserved.
// Licensed under the Apache License, Version 2.0.

using System.Linq;
using System.Threading.Tasks;
using Authen.Users.Constants;
using AuthenApi.Dtos.Configuration;
using AuthenApi.ExceptionHandling;
using AuthenApi.Helpers;
using AuthenApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Authen.Controllers
{
    [Authorize(Policy = AuthorizationConsts.AdministrationPolicy)]
    [TypeFilter(typeof(ControllerExceptionFilterAttribute))]
    public class ConfigurationController : BaseController
    {
        private readonly IIdentityResourceService _identityResourceService;
        private readonly IApiResourceService _apiResourceService;
        private readonly IClientService _clientService;
        private readonly IStringLocalizer<ConfigurationController> _localizer;
        private readonly IApiScopeService _apiScopeService;

        public ConfigurationController(IIdentityResourceService identityResourceService,
            IApiResourceService apiResourceService,
            IClientService clientService,
            IStringLocalizer<ConfigurationController> localizer,
            ILogger<ConfigurationController> logger,
            IApiScopeService apiScopeService)
            : base(logger)
        {
            _identityResourceService = identityResourceService;
            _apiResourceService = apiResourceService;
            _clientService = clientService;
            _localizer = localizer;
            _apiScopeService = apiScopeService;
        }

        public async Task<IActionResult> Client(string id)
        {
            if (id.IsNotPresentedValidNumber())
            {
                return NotFound();
            }

            if (id == default)
            {
                var clientDto = _clientService.BuildClientViewModel();
                return View(clientDto);
            }

            int.TryParse(id, out var clientId);
            var client = await _clientService.GetClientAsync(clientId);
            client = _clientService.BuildClientViewModel(client);

            return View(client);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Client(ClientDto client)
        {
            client = _clientService.BuildClientViewModel(client);

            if (!ModelState.IsValid)
            {
                return View(client);
            }

            //Add new client
            if (client.Id == 0)
            {
                var clientId = await _clientService.AddClientAsync(client);
                SuccessNotification(string.Format(_localizer["SuccessAddClient"], client.ClientId), _localizer["SuccessTitle"]);

                return RedirectToAction(nameof(Client), new { Id = clientId });
            }

            //Update client
            await _clientService.UpdateClientAsync(client);
            SuccessNotification(string.Format(_localizer["SuccessUpdateClient"], client.ClientId), _localizer["SuccessTitle"]);

            return RedirectToAction(nameof(Client), new { client.Id });
        }

        [HttpGet]
        public async Task<IActionResult> ClientClone(int id)
        {
            if (id == 0) return NotFound();

            var clientDto = await _clientService.GetClientAsync(id);
            var client = _clientService.BuildClientCloneViewModel(id, clientDto);

            return View(client);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ClientClone(ClientCloneDto client)
        {
            if (!ModelState.IsValid)
            {
                return View(client);
            }

            var newClientId = await _clientService.CloneClientAsync(client);
            SuccessNotification(string.Format(_localizer["SuccessClientClone"], client.ClientId), _localizer["SuccessTitle"]);

            return RedirectToAction(nameof(Client), new { Id = newClientId });
        }

        [HttpGet]
        public async Task<IActionResult> ClientDelete(int id)
        {
            if (id == 0) return NotFound();

            var client = await _clientService.GetClientAsync(id);

            return View(client);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ClientDelete(ClientDto client)
        {
            await _clientService.RemoveClientAsync(client);

            SuccessNotification(_localizer["SuccessClientDelete"], _localizer["SuccessTitle"]);

            return RedirectToAction(nameof(Clients));
        }

        [HttpGet]
        public async Task<IActionResult> ClientClaims(int id, int? page)
        {
            if (id == 0) return NotFound();

            var claims = await _clientService.GetClientClaimsAsync(id, page ?? 1);

            return View(claims);
        }

        [HttpGet]
        public async Task<IActionResult> ClientProperties(int id, int? page)
        {
            if (id == 0) return NotFound();

            var properties = await _clientService.GetClientPropertiesAsync(id, page ?? 1);

            return View(properties);
        }

        [HttpGet]
        public async Task<IActionResult> ApiResourceProperties(int id, int? page)
        {
            if (id == 0) return NotFound();

            var properties = await _apiResourceService.GetApiResourcePropertiesAsync(id, page ?? 1);

            return View(properties);
        }

        [HttpGet]
        public async Task<IActionResult> ApiScopeProperties(int id, int? page)
        {
            if (id == 0) return NotFound();

            var properties = await _apiScopeService.GetApiScopePropertiesAsync(id, page ?? 1);

            return View(properties);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApiScopeProperties(ApiScopePropertiesDto apiScopeProperty)
        {
            if (!ModelState.IsValid)
            {
                return View(apiScopeProperty);
            }

            await _apiScopeService.AddApiScopePropertyAsync(apiScopeProperty);
            SuccessNotification(string.Format(_localizer["SuccessAddApiScopeProperty"], apiScopeProperty.Key, apiScopeProperty.ApiScopeName), _localizer["SuccessTitle"]);

            return RedirectToAction(nameof(ApiScopeProperties), new { Id = apiScopeProperty.ApiScopeId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApiResourceProperties(ApiResourcePropertiesDto apiResourceProperty)
        {
            if (!ModelState.IsValid)
            {
                return View(apiResourceProperty);
            }

            await _apiResourceService.AddApiResourcePropertyAsync(apiResourceProperty);
            SuccessNotification(string.Format(_localizer["SuccessAddApiResourceProperty"], apiResourceProperty.Key, apiResourceProperty.ApiResourceName), _localizer["SuccessTitle"]);

            return RedirectToAction(nameof(ApiResourceProperties), new { Id = apiResourceProperty.ApiResourceId });
        }

        [HttpGet]
        public async Task<IActionResult> IdentityResourceProperties(int id, int? page)
        {
            if (id == 0) return NotFound();

            var properties = await _identityResourceService.GetIdentityResourcePropertiesAsync(id, page ?? 1);

            return View(properties);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IdentityResourceProperties(IdentityResourcePropertiesDto identityResourceProperty)
        {
            if (!ModelState.IsValid)
            {
                return View(identityResourceProperty);
            }

            await _identityResourceService.AddIdentityResourcePropertyAsync(identityResourceProperty);
            SuccessNotification(string.Format(_localizer["SuccessAddIdentityResourceProperty"], identityResourceProperty.Value, identityResourceProperty.IdentityResourceName), _localizer["SuccessTitle"]);

            return RedirectToAction(nameof(IdentityResourceProperties), new { Id = identityResourceProperty.IdentityResourceId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ClientProperties(ClientPropertiesDto clientProperty)
        {
            if (!ModelState.IsValid)
            {
                return View(clientProperty);
            }

            await _clientService.AddClientPropertyAsync(clientProperty);
            SuccessNotification(string.Format(_localizer["SuccessAddClientProperty"], clientProperty.ClientId, clientProperty.ClientName), _localizer["SuccessTitle"]);

            return RedirectToAction(nameof(ClientProperties), new { Id = clientProperty.ClientId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ClientClaims(ClientClaimsDto clientClaim)
        {
            if (!ModelState.IsValid)
            {
                return View(clientClaim);
            }

            await _clientService.AddClientClaimAsync(clientClaim);
            SuccessNotification(string.Format(_localizer["SuccessAddClientClaim"], clientClaim.Value, clientClaim.ClientName), _localizer["SuccessTitle"]);

            return RedirectToAction(nameof(ClientClaims), new { Id = clientClaim.ClientId });
        }

        [HttpGet]
        public async Task<IActionResult> ClientClaimDelete(int id)
        {
            if (id == 0) return NotFound();

            var clientClaim = await _clientService.GetClientClaimAsync(id);

            return View(nameof(ClientClaimDelete), clientClaim);
        }

        [HttpGet]
        public async Task<IActionResult> ClientPropertyDelete(int id)
        {
            if (id == 0) return NotFound();

            var clientProperty = await _clientService.GetClientPropertyAsync(id);

            return View(nameof(ClientPropertyDelete), clientProperty);
        }

        [HttpGet]
        public async Task<IActionResult> ApiResourcePropertyDelete(int id)
        {
            if (id == 0) return NotFound();

            var apiResourceProperty = await _apiResourceService.GetApiResourcePropertyAsync(id);

            return View(nameof(ApiResourcePropertyDelete), apiResourceProperty);
        }

        [HttpGet]
        public async Task<IActionResult> ApiScopePropertyDelete(int id)
        {
            if (id == 0) return NotFound();

            var apiScopeProperty = await _apiScopeService.GetApiScopePropertyAsync(id);

            return View(nameof(ApiScopePropertyDelete), apiScopeProperty);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApiScopePropertyDelete(ApiScopePropertiesDto apiScopeProperty)
        {
            await _apiScopeService.DeleteApiScopePropertyAsync(apiScopeProperty);
            SuccessNotification(_localizer["SuccessDeleteApiScopeProperty"], _localizer["SuccessTitle"]);

            return RedirectToAction(nameof(ApiScopeProperties), new { Id = apiScopeProperty.ApiScopeId });
        }

        [HttpGet]
        public async Task<IActionResult> IdentityResourcePropertyDelete(int id)
        {
            if (id == 0) return NotFound();

            var identityResourceProperty = await _identityResourceService.GetIdentityResourcePropertyAsync(id);

            return View(nameof(IdentityResourcePropertyDelete), identityResourceProperty);
        }

        [HttpPost]
        public async Task<IActionResult> ClientClaimDelete(ClientClaimsDto clientClaim)
        {
            await _clientService.DeleteClientClaimAsync(clientClaim);
            SuccessNotification(_localizer["SuccessDeleteClientClaim"], _localizer["SuccessTitle"]);

            return RedirectToAction(nameof(ClientClaims), new { Id = clientClaim.ClientId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ClientPropertyDelete(ClientPropertiesDto clientProperty)
        {
            await _clientService.DeleteClientPropertyAsync(clientProperty);
            SuccessNotification(_localizer["SuccessDeleteClientProperty"], _localizer["SuccessTitle"]);

            return RedirectToAction(nameof(ClientProperties), new { Id = clientProperty.ClientId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApiResourcePropertyDelete(ApiResourcePropertiesDto apiResourceProperty)
        {
            await _apiResourceService.DeleteApiResourcePropertyAsync(apiResourceProperty);
            SuccessNotification(_localizer["SuccessDeleteApiResourceProperty"], _localizer["SuccessTitle"]);

            return RedirectToAction(nameof(ApiResourceProperties), new { Id = apiResourceProperty.ApiResourceId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IdentityResourcePropertyDelete(IdentityResourcePropertiesDto identityResourceProperty)
        {
            await _identityResourceService.DeleteIdentityResourcePropertyAsync(identityResourceProperty);
            SuccessNotification(_localizer["SuccessDeleteIdentityResourceProperty"], _localizer["SuccessTitle"]);

            return RedirectToAction(nameof(IdentityResourceProperties), new { Id = identityResourceProperty.IdentityResourceId });
        }

        [HttpGet]
        public async Task<IActionResult> ClientSecrets(int id, int? page)
        {
            if (id == 0) return NotFound();

            var clientSecrets = await _clientService.GetClientSecretsAsync(id, page ?? 1);
            _clientService.BuildClientSecretsViewModel(clientSecrets);

            return View(clientSecrets);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ClientSecrets(ClientSecretsDto clientSecret)
        {
            await _clientService.AddClientSecretAsync(clientSecret);
            SuccessNotification(string.Format(_localizer["SuccessAddClientSecret"], clientSecret.ClientName), _localizer["SuccessTitle"]);

            return RedirectToAction(nameof(ClientSecrets), new { Id = clientSecret.ClientId });
        }

        [HttpGet]
        public async Task<IActionResult> ClientSecretDelete(int id)
        {
            if (id == 0) return NotFound();

            var clientSecret = await _clientService.GetClientSecretAsync(id);

            return View(nameof(ClientSecretDelete), clientSecret);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ClientSecretDelete(ClientSecretsDto clientSecret)
        {
            await _clientService.DeleteClientSecretAsync(clientSecret);
            SuccessNotification(_localizer["SuccessDeleteClientSecret"], _localizer["SuccessTitle"]);

            return RedirectToAction(nameof(ClientSecrets), new { Id = clientSecret.ClientId });
        }

        [HttpGet]
        public async Task<IActionResult> SearchScopes(string scope, int limit = 0)
        {
            var scopes = await _clientService.GetScopesAsync(scope, limit);

            return Ok(scopes);
        }

        [HttpGet]
        public async Task<IActionResult> SearchApiScopes(string scope, int limit = 0)
        {
            var scopes = await _apiScopeService.GetApiScopesNameAsync(scope, limit);

            return Ok(scopes.ToList());
        }

        [HttpGet]
        public IActionResult SearchClaims(string claim, int limit = 0)
        {
            var claims = _clientService.GetStandardClaims(claim, limit);

            return Ok(claims);
        }

        [HttpGet]
        public IActionResult SearchSigningAlgorithms(string algorithm, int limit = 0)
        {
            var signingAlgorithms = _clientService.GetSigningAlgorithms(algorithm, limit);

            return Ok(signingAlgorithms);
        }


        [HttpGet]
        public IActionResult SearchGrantTypes(string grant, int limit = 0)
        {
            var grants = _clientService.GetGrantTypes(grant, true, limit).Select(x => x.Id).ToList();

            return Ok(grants);
        }

        [HttpGet]
        public async Task<IActionResult> Clients(int? page, string search)
        {
            ViewBag.Search = search;
            return View(await _clientService.GetClientsAsync(search, page ?? 1));
        }

        [HttpGet]
        public async Task<IActionResult> IdentityResourceDelete(int id)
        {
            if (id == 0) return NotFound();

            var identityResource = await _identityResourceService.GetIdentityResourceAsync(id);

            return View(identityResource);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IdentityResourceDelete(IdentityResourceDto identityResource)
        {
            await _identityResourceService.DeleteIdentityResourceAsync(identityResource);
            SuccessNotification(_localizer["SuccessDeleteIdentityResource"], _localizer["SuccessTitle"]);

            return RedirectToAction(nameof(IdentityResources));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IdentityResource(IdentityResourceDto identityResource)
        {
            if (!ModelState.IsValid)
            {
                return View(identityResource);
            }

            identityResource = _identityResourceService.BuildIdentityResourceViewModel(identityResource);

            int identityResourceId;

            if (identityResource.Id == 0)
            {
                identityResourceId = await _identityResourceService.AddIdentityResourceAsync(identityResource);
            }
            else
            {
                identityResourceId = identityResource.Id;
                await _identityResourceService.UpdateIdentityResourceAsync(identityResource);
            }

            SuccessNotification(string.Format(_localizer["SuccessAddIdentityResource"], identityResource.Name), _localizer["SuccessTitle"]);

            return RedirectToAction(nameof(IdentityResource), new { Id = identityResourceId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApiResource(ApiResourceDto apiResource)
        {
            if (!ModelState.IsValid)
            {
                return View(apiResource);
            }

            ComboBoxHelpers.PopulateValuesToList(apiResource.UserClaimsItems, apiResource.UserClaims);
            ComboBoxHelpers.PopulateValuesToList(apiResource.ScopesItems, apiResource.Scopes);
            ComboBoxHelpers.PopulateValuesToList(apiResource.AllowedAccessTokenSigningAlgorithmsItems, apiResource.AllowedAccessTokenSigningAlgorithms);

            int apiResourceId;

            if (apiResource.Id == 0)
            {
                apiResourceId = await _apiResourceService.AddApiResourceAsync(apiResource);
            }
            else
            {
                apiResourceId = apiResource.Id;
                await _apiResourceService.UpdateApiResourceAsync(apiResource);
            }

            SuccessNotification(string.Format(_localizer["SuccessAddApiResource"], apiResource.Name), _localizer["SuccessTitle"]);

            return RedirectToAction(nameof(ApiResource), new { Id = apiResourceId });
        }

        [HttpGet]
        public async Task<IActionResult> ApiResourceDelete(int id)
        {
            if (id == 0) return NotFound();

            var apiResource = await _apiResourceService.GetApiResourceAsync(id);

            return View(apiResource);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApiResourceDelete(ApiResourceDto apiResource)
        {
            await _apiResourceService.DeleteApiResourceAsync(apiResource);
            SuccessNotification(_localizer["SuccessDeleteApiResource"], _localizer["SuccessTitle"]);

            return RedirectToAction(nameof(ApiResources));
        }

        [HttpGet]
        public async Task<IActionResult> ApiResource(string id)
        {
            if (id.IsNotPresentedValidNumber())
            {
                return NotFound();
            }

            if (id == default)
            {
                var apiResourceDto = new ApiResourceDto();
                return View(apiResourceDto);
            }

            int.TryParse(id, out var apiResourceId);
            var apiResource = await _apiResourceService.GetApiResourceAsync(apiResourceId);

            return View(apiResource);
        }

        [HttpGet]
        public async Task<IActionResult> ApiSecrets(int id, int? page)
        {
            if (id == 0) return NotFound();

            var apiSecrets = await _apiResourceService.GetApiSecretsAsync(id, page ?? 1);
            _apiResourceService.BuildApiSecretsViewModel(apiSecrets);

            return View(apiSecrets);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApiSecrets(ApiSecretsDto apiSecret)
        {
            if (!ModelState.IsValid)
            {
                return View(apiSecret);
            }

            await _apiResourceService.AddApiSecretAsync(apiSecret);
            SuccessNotification(_localizer["SuccessAddApiSecret"], _localizer["SuccessTitle"]);

            return RedirectToAction(nameof(ApiSecrets), new { Id = apiSecret.ApiResourceId });
        }

        [HttpGet]
        public async Task<IActionResult> ApiScopes(string search, int? page)
        {
            ViewBag.Search = search;
            var apiScopesDto = await _apiScopeService.GetApiScopesAsync(search, page ?? 1);

            return View(apiScopesDto);
        }

        [HttpGet]
        public async Task<IActionResult> ApiScope(string id)
        {
            if (id.IsNotPresentedValidNumber())
            {
                return NotFound();
            }

            if (id == default)
            {
                var apiScopeDto = new ApiScopeDto();

                return View(apiScopeDto);
            }
            else
            {
                int.TryParse(id, out var apiScopeId);
                var apiScopeDto = await _apiScopeService.GetApiScopeAsync(apiScopeId);

                return View(apiScopeDto);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApiScope(ApiScopeDto apiScope)
        {
            if (!ModelState.IsValid)
            {
                return View(apiScope);
            }

            _apiScopeService.BuildApiScopeViewModel(apiScope);

            int apiScopeId;

            if (apiScope.Id == 0)
            {
                apiScopeId = await _apiScopeService.AddApiScopeAsync(apiScope);
            }
            else
            {
                apiScopeId = apiScope.Id;
                await _apiScopeService.UpdateApiScopeAsync(apiScope);
            }

            SuccessNotification(string.Format(_localizer["SuccessAddApiScope"], apiScope.Name), _localizer["SuccessTitle"]);

            return RedirectToAction(nameof(ApiScope), new { id = apiScopeId });
        }

        [HttpGet]
        public async Task<IActionResult> ApiScopeDelete(int id)
        {
            if (id == 0) return NotFound();

            var apiScope = await _apiScopeService.GetApiScopeAsync(id);

            return View(nameof(ApiScopeDelete), apiScope);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApiScopeDelete(ApiScopeDto apiScope)
        {
            await _apiScopeService.DeleteApiScopeAsync(apiScope);
            SuccessNotification(_localizer["SuccessDeleteApiScope"], _localizer["SuccessTitle"]);

            return RedirectToAction(nameof(ApiScopes));
        }

        [HttpGet]
        public async Task<IActionResult> ApiResources(int? page, string search)
        {
            ViewBag.Search = search;
            var apiResources = await _apiResourceService.GetApiResourcesAsync(search, page ?? 1);

            return View(apiResources);
        }

        [HttpGet]
        public async Task<IActionResult> IdentityResources(int? page, string search)
        {
            ViewBag.Search = search;
            var identityResourcesDto = await _identityResourceService.GetIdentityResourcesAsync(search, page ?? 1);

            return View(identityResourcesDto);
        }

        [HttpGet]
        public async Task<IActionResult> ApiSecretDelete(int id)
        {
            if (id == 0) return NotFound();

            var clientSecret = await _apiResourceService.GetApiSecretAsync(id);

            return View(nameof(ApiSecretDelete), clientSecret);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApiSecretDelete(ApiSecretsDto apiSecret)
        {
            await _apiResourceService.DeleteApiSecretAsync(apiSecret);
            SuccessNotification(_localizer["SuccessDeleteApiSecret"], _localizer["SuccessTitle"]);

            return RedirectToAction(nameof(ApiSecrets), new { Id = apiSecret.ApiResourceId });
        }

        [HttpGet]
        public async Task<IActionResult> IdentityResource(string id)
        {
            if (id.IsNotPresentedValidNumber())
            {
                return NotFound();
            }

            if (id == default)
            {
                var identityResourceDto = new IdentityResourceDto();
                return View(identityResourceDto);
            }

            int.TryParse(id, out var identityResourceId);
            var identityResource = await _identityResourceService.GetIdentityResourceAsync(identityResourceId);

            return View(identityResource);
        }
    }
}