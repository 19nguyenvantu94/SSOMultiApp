﻿// Copyright (c) Jan Škoruba. All Rights Reserved.
// Licensed under the Apache License, Version 2.0.

using System.Threading.Tasks;
using Authen.Users.Constants;
using AuthenApi.Dtos.Key;
using AuthenApi.ExceptionHandling;
using AuthenApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Authen.Controllers
{
    [Authorize(Policy = AuthorizationConsts.AdministrationPolicy)]
    [TypeFilter(typeof(ControllerExceptionFilterAttribute))]
    public class KeyController : BaseController
    {
        private readonly IKeyService _keyService;
        private readonly IStringLocalizer<KeyController> _localizer;

        public KeyController(IKeyService keyService,
            ILogger<KeyController> logger,
            IStringLocalizer<KeyController> localizer) : base(logger)
        {
            _keyService = keyService;
            _localizer = localizer;
        }

        [HttpGet]
        public async Task<IActionResult> Keys(int? page)
        {
            var keys = await _keyService.GetKeysAsync(page ?? 1);

            return View(keys);
        }

        [HttpGet]
        public async Task<IActionResult> KeyDelete(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var key = await _keyService.GetKeyAsync(id);
            if (key == null) return NotFound();

            return View(key);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> KeyDelete(KeyDto key)
        {
            await _keyService.DeleteKeyAsync(key.Id);

            SuccessNotification(_localizer["Key successfully deleted"], _localizer["Key Deleted"]);

            return RedirectToAction(nameof(Keys));
        }
    }
}