using Authen;
using Authen.Authorization;
using Authen.Configuration.Email;
using Authen.Constants;
using Authen.Data;
using Authen.Helpers.Localization;
using Authen.Localization;
using Authen.Repositories;
using Authen.Repositories.Interfaces;
using Authen.Resources;
using Authen.Server.Aop;
using Authen.Users.Constants;
using Authen.Users.Data;
using Authen.Users.Models;
using Authen.Users.Services;
using AuthenApi.Mappers;
using AuthenApi.Repositories;
using AuthenApi.Repositories.Interfaces;
using AuthenApi.Resources;
using AuthenApi.Services;
using AuthenApi.Services.Interfaces;
using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using SendGrid;
using Serilog;
using Serilog.Events;
using Serilog.Extensions.Logging;
using Serilog.Sinks.SystemConsole.Themes;
using SharedLib.DistributedRedis;
using SharedLib.MySQL;
using System.Diagnostics;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

Activity.DefaultIdFormat = ActivityIdFormat.W3C;

Serilog.Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
    .MinimumLevel.Override("System", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Code)
    .CreateLogger();

builder.Logging.AddSerilog();

var configuration = builder.Configuration;

builder.AddRedisDistributedCache("Redis");

//builder.AddRabbitMqEventBus("EventBus");
builder.AddMySqlDbContext<ApplicationDbContext>("Identitydb");
builder.AddMySqlDbContext<TenantStoreDbContext>("Identitydb");

builder.Services.AddSingleton<RedisUserRepository>();


builder.Services.AddControllers(options =>
{
    var parameterTransformer = new SlugifyParameterTransformer();
    options.Conventions.Add(new CustomActionNameConvention(parameterTransformer));
    options.Conventions.Add(new RouteTokenTransformerConvention(parameterTransformer));
});

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(44360, listenOptions =>
    {
        listenOptions.UseHttps("identityserver.pfx", "nguyenvantu123");
    });
});
//builder.Services.AddAuthorizationPolicies(options.Admin, Security.AuthorizationConfigureAction); 
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(AuthorizationConsts.AdministrationPolicy,
        policy => policy.RequireRole(DefaultRoleNames.Administrator));

    //authorizationAction?.Invoke(options);
});

//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy(AuthorizationConsts.AdministrationPolicy,
//        policy =>
//            policy.RequireRole(DefaultRoleNames.Administrator);
//});


var profileTypes = new HashSet<Type>
            {
                typeof(IdentityMapperProfile)
            };

builder.Services.AddAdminAspNetIdentityMapping()
            .UseIdentityMappingProfile()
            .AddProfilesType(profileTypes);

builder.Services.AddAuthorization();

builder.Services.AddSingleton<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();
builder.Services.AddTransient<IAuthorizationHandler, DomainRequirementHandler>();
builder.Services.AddTransient<IAuthorizationHandler, EmailVerifiedHandler>();
builder.Services.AddTransient<IAuthorizationHandler, PermissionRequirementHandler>();

builder.Services.AddMultiTenant<AppTenantInfo>()
    .WithHostStrategy("__tenant__")
    .WithEFCoreStore<TenantStoreDbContext, AppTenantInfo>()
    .WithStaticStrategy(DefaultTenant.DefaultTenantId);

builder.Services.AddRazorPages();

//builder.Services.AddSingleton<DatabaseInitializer>();
builder.Services.Replace(new ServiceDescriptor(typeof(ITenantResolver<AppTenantInfo>), typeof(TenantResolver<AppTenantInfo>), ServiceLifetime.Scoped));

builder.Services.Replace(new ServiceDescriptor(typeof(ITenantResolver), sp => sp.GetRequiredService<ITenantResolver<AppTenantInfo>>(), ServiceLifetime.Scoped));

//builder.AddMySqlDataSource("Identitydb");
//builder.AddSqlServerDbContext<ApplicationDbContext>("Identitydb");

builder.Services.AddTransient<IDatabaseInitializer, DatabaseInitializer>();
//var withApiVersioning = builder.Services.AddApiVersioning();

//builder.AddDefaultOpenApi(withApiVersioning);

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

var smtpConfiguration = configuration.GetSection(nameof(SmtpConfiguration)).Get<SmtpConfiguration>();
var sendGridConfiguration = configuration.GetSection(nameof(SendGridConfiguration)).Get<SendGridConfiguration>();

if (sendGridConfiguration != null && !string.IsNullOrWhiteSpace(sendGridConfiguration.ApiKey))
{
    builder.Services.AddSingleton<ISendGridClient>(_ => new SendGridClient(sendGridConfiguration.ApiKey));
    builder.Services.AddSingleton(sendGridConfiguration);
    builder.Services.AddTransient<IEmailSender, SendGridEmailSender>();
}
else if (smtpConfiguration != null && !string.IsNullOrWhiteSpace(smtpConfiguration.Host))
{
    builder.Services.AddSingleton(smtpConfiguration);
    builder.Services.AddTransient<IEmailSender, SmtpEmailSender>();
}
else
{
    builder.Services.AddSingleton<IEmailSender, LogEmailSender>();
}

builder.Services.AddTransient<IIdentityService, IdentityService>();
builder.Services.AddScoped<IIdentityServiceResources, IdentityServiceResources>();
builder.Services.AddTransient<IIdentityRepository, IdentityRepository>();

//Repositories

//Services
//services.AddTransient<IPersistedGrantAspNetIdentityService, PersistedGrantAspNetIdentityService>();
//services.AddTransient<IDashboardIdentityService, DashboardIdentityService>();

//Resources

builder.Services.AddScoped<IPersistedGrantAspNetIdentityRepository, PersistedGrantAspNetIdentityRepository>();
builder.Services.AddScoped<IPersistedGrantAspNetIdentityServiceResources, PersistedGrantAspNetIdentityServiceResources>();

builder.Services.AddTransient<IPersistedGrantAspNetIdentityService, PersistedGrantAspNetIdentityService>();


builder.Services.AddTransient<IIdentityRepository, IdentityRepository>();

builder.Services.AddTransient<IClientRepository, ClientRepository>();
builder.Services.AddTransient<IIdentityResourceRepository, IdentityResourceRepository>();
builder.Services.AddTransient<IApiResourceRepository, ApiResourceRepository>();
builder.Services.AddTransient<IApiScopeRepository, ApiScopeRepository>();
builder.Services.AddTransient<IPersistedGrantRepository, PersistedGrantRepository>();
builder.Services.AddTransient<IIdentityProviderRepository, IdentityProviderRepository>();
builder.Services.AddTransient<IKeyRepository, KeyRepository>();
//builder.Services.AddTransient<ILogRepository, LogRepository<TLogDbContext>>();
builder.Services.AddTransient<IDashboardRepository, DashboardRepository>();
builder.Services.AddTransient<IClaimsPoliciesRepository, ClaimsPoliciesRepository>();



//Services
builder.Services.AddTransient<IClientService, ClientService>();
builder.Services.AddTransient<IApiResourceService, ApiResourceService>();
builder.Services.AddTransient<IApiScopeService, ApiScopeService>();
builder.Services.AddTransient<IIdentityResourceService, IdentityResourceService>();
builder.Services.AddTransient<IIdentityProviderService, IdentityProviderService>();
builder.Services.AddTransient<IPersistedGrantService, PersistedGrantService>();
builder.Services.AddTransient<IKeyService, KeyService>();
builder.Services.AddTransient<IDashboardService, DashboardService>();

builder.Services.AddTransient<IClaimsPoliciesService, ClaimPoliciesService>();

builder.Services.AddTransient<IClaimsPoliciesServiceResources, ClaimsPoliciesServiceResources>();

//builder.Services.AddTransient<IIdentityService, IdentityService>();

//Resources
builder.Services.AddScoped<IApiResourceServiceResources, ApiResourceServiceResources>();
builder.Services.AddScoped<IApiScopeServiceResources, ApiScopeServiceResources>();
builder.Services.AddScoped<IClientServiceResources, ClientServiceResources>();
builder.Services.AddScoped<IIdentityResourceServiceResources, IdentityResourceServiceResources>();
builder.Services.AddScoped<IIdentityProviderServiceResources, IdentityProviderServiceResources>();
builder.Services.AddScoped<IPersistedGrantServiceResources, PersistedGrantServiceResources>();
builder.Services.AddScoped<IKeyServiceResources, KeyServiceResources>();

builder.Services.AddTransient<IIdentityService, IdentityService>();

builder.Services.AddLocalization(opts => { opts.ResourcesPath = ConfigurationConsts.ResourcesPath; });

builder.Services.TryAddTransient(typeof(IGenericControllerLocalizer<>), typeof(GenericControllerLocalizer<>));

builder.Services.AddControllersWithViews(o =>
{
    o.Conventions.Add(new GenericControllerRouteConvention());
})
    .AddViewLocalization(
        LanguageViewLocationExpanderFormat.Suffix,
        opts => { opts.ResourcesPath = ConfigurationConsts.ResourcesPath; })
    .AddDataAnnotationsLocalization()
    .ConfigureApplicationPartManager(m =>
    {
        m.FeatureProviders.Add(new GenericTypeControllerFeatureProvider<ApplicationUser, Guid>());
    });

var cultureConfiguration = configuration.GetSection(nameof(CultureConfiguration)).Get<CultureConfiguration>();
builder.Services.Configure<RequestLocalizationOptions>(
    opts =>
    {
        // If cultures are specified in the configuration, use them (making sure they are among the available cultures),
        // otherwise use all the available cultures
        var supportedCultureCodes = (cultureConfiguration?.Cultures?.Count > 0 ?
            cultureConfiguration.Cultures.Intersect(CultureConfiguration.AvailableCultures) :
            CultureConfiguration.AvailableCultures).ToArray();

        if (!supportedCultureCodes.Any()) supportedCultureCodes = CultureConfiguration.AvailableCultures;
        var supportedCultures = supportedCultureCodes.Select(c => new CultureInfo(c)).ToList();

        // If the default culture is specified use it, otherwise use CultureConfiguration.DefaultRequestCulture ("en")
        var defaultCultureCode = string.IsNullOrEmpty(cultureConfiguration?.DefaultCulture) ?
            CultureConfiguration.DefaultRequestCulture : cultureConfiguration?.DefaultCulture;

        // If the default culture is not among the supported cultures, use the first supported culture as default
        if (!supportedCultureCodes.Contains(defaultCultureCode)) defaultCultureCode = supportedCultureCodes.FirstOrDefault();

        opts.DefaultRequestCulture = new RequestCulture(defaultCultureCode);
        opts.SupportedCultures = supportedCultures;
        opts.SupportedUICultures = supportedCultures;
    });

builder.Services.AddIdentityServer()
                .AddConfigurationStore<ApplicationDbContext>()
                .AddOperationalStore<ApplicationDbContext>()
                .AddAspNetIdentity<ApplicationUser>()
                .AddProfileService<ProfileService>();

//builder.Services.AddIdentityServer()
//.AddInMemoryIdentityResources(Config.GetResources())
//.AddInMemoryApiScopes(Config.GetApiScopes())
//.AddInMemoryApiResources(Config.GetApis())
//.AddInMemoryClients(Config.GetClients(builder.Configuration))
//.AddAspNetIdentity<ApplicationUser>()
//.AddProfileService<ProfileService>()
//// TODO: Not recommended for production - you need to store your key material somewhere secure
//.AddDeveloperSigningCredential();

builder.Services.AddScoped<EntityPermissions>();
//builder.Services.AddTransient<IProfileService, ProfileService>();
builder.Services.AddTransient<ILoginService<ApplicationUser>, EFLoginService>();
builder.Services.AddTransient<IRedirectService, RedirectService>();
builder.Services.AddTransient<IEmailFactory, EmailFactory>();
//builder.Services.AddSingleton<CustomAuthService>();

builder.Services.AddSingleton<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();
builder.Services.AddTransient<IAuthorizationHandler, DomainRequirementHandler>();
builder.Services.AddTransient<IAuthorizationHandler, EmailVerifiedHandler>();
builder.Services.AddTransient<IAuthorizationHandler, PermissionRequirementHandler>();

//builder.Services.AddSingleton<IViewLocalizer>();

builder.Services.AddTransient<ApiResponseExceptionAspect>()
                .AddTransient<LogExceptionAspect>()
                .AddSingleton<ILoggerFactory>(services => new SerilogLoggerFactory());

builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.AddMvc().AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix);

builder.Services.AddControllersWithViews(o =>
{
    o.Conventions.Add(new GenericControllerRouteConvention());
})
                .AddViewLocalization(
                    LanguageViewLocationExpanderFormat.Suffix,
                    opts => { opts.ResourcesPath = "Resources"; })
                .AddDataAnnotationsLocalization()
                .ConfigureApplicationPartManager(m =>
                {
                    m.FeatureProviders.Add(new GenericTypeControllerFeatureProvider<ApplicationUser, Guid>());
                });

var app = builder.Build();

app.MapDefaultEndpoints();

app.UseStaticFiles();

app.UseCookiePolicy(new CookiePolicyOptions { MinimumSameSitePolicy = SameSiteMode.Lax });
app.UseRouting();

app.UseIdentityServer();
app.UseAuthorization();
app.UseAuthentication();

app.MapDefaultControllerRoute();

app.UseDeveloperExceptionPage();
app.UseMultiTenant();

app.Use((context, next) =>
{
    if (context.Request.Host.Host != "localhost")
        return context.Response.WriteAsync("Bad request");

    return next();
});


//app.UseMiddleware<UserSessionMiddleware>();

using (var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
{

    var databaseInitializer = serviceScope.ServiceProvider.GetService<IDatabaseInitializer>();
    databaseInitializer.SeedAsync().Wait();
}

var options = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(options.Value);
app.Run();






//// Add services to the container.
//builder.Services.AddControllersWithViews();

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}

//app.UseHttpsRedirection();
//app.UseStaticFiles();

//app.UseRouting();

//app.UseAuthorization();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");

//app.Run();
