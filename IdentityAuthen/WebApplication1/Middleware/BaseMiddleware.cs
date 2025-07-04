﻿using Authen.Users.Models;
using Breeze.Persistence;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using ObjectCloner.Extensions;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace Authen.Users.Middleware
{
    public abstract class BaseMiddleware
    {
        protected ILogger<BaseMiddleware> _logger;

        //https://trailheadtechnology.com/aspnetcore-multi-tenant-tips-and-tricks/
        protected readonly RequestDelegate _next;

        protected BaseMiddleware(RequestDelegate next, ILogger<BaseMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        protected Task RewriteResponseAsApiResponse(HttpContext httpContext, ApiResponse apiResponse)
        {
            var headers = httpContext.Response.Headers.DeepClone();
            httpContext.Response.Clear();

            foreach (var h in headers.Where(i => !i.Key.StartsWith("Content")))
                httpContext.Response.Headers.Add(h.Key, h.Value);

            httpContext.Response.ContentType = "application/json";

            return httpContext.Response.WriteAsync(JsonConvert.SerializeObject(apiResponse));
        }

        protected async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
        {
            string msg = exception.GetBaseException().StackTrace;
            string userMsg = "Operation Failed";
            int code = Status500InternalServerError;

            if (exception is EntityErrorsException)
            {
                return;
            }
            else if (exception is UnauthorizedAccessException)
            {
                userMsg = msg = "UnauthorizedAccess";
                code = Status401Unauthorized;
            }
            else if (exception is DomainException exception1)
            {
                userMsg = msg = exception1.Description;
            }

            _logger.LogError($"Api Exception: {msg}");

            httpContext.Response.StatusCode = code;

            await RewriteResponseAsApiResponse(httpContext, new ApiResponse(Status500InternalServerError, userMsg));
        }

    }
}
