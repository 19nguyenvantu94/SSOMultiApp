using Authen.Data;
using AuthenApi.Mappers.Configuration;
using AutoMapper;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;


internal static class Extensions
{
    //public static void AddApplicationServices(this IHostApplicationBuilder builder)
    //{
    //    builder.AddDefaultAuthentication();

    //    //builder.AddRabbitMqEventBus("EventBus").AddEventBusSubscriptions();

    //    builder.AddSqlServerDbContext<ApplicationDbContext>("Identitydb");

    //    //builder.Services.AddMigration<WebhooksContext>();

    //    //builder.Services.AddTransient<IGrantUrlTesterService, GrantUrlTesterService>();
    //    //builder.Services.AddTransient<IWebhooksRetriever, WebhooksRetriever>();
    //    //builder.Services.AddTransient<IWebhooksSender, WebhooksSender>();
    //}

    public static IMapperConfigurationBuilder AddAdminAspNetIdentityMapping(this IServiceCollection services)
    {
        var builder = new MapperConfigurationBuilder();

        services.AddSingleton<AutoMapper.IConfigurationProvider>(sp => new MapperConfiguration(cfg =>
        {
            foreach (var profileType in builder.ProfileTypes)
                cfg.AddProfile(profileType);
        }));

        services.AddScoped<IMapper>(sp => new Mapper(sp.GetRequiredService<AutoMapper.IConfigurationProvider>(), sp.GetService));

        return builder;
    }

    public static WebApplication MapDefaultEndpoints(this WebApplication app)
    {
        // Adding health checks endpoints to applications in non-development environments has security implications.
        // See https://aka.ms/dotnet/aspire/healthchecks for details before enabling these endpoints in non-development environments.
        if (app.Environment.IsDevelopment())
        {
            // All health checks must pass for app to be considered ready to accept traffic after starting
            app.MapHealthChecks("/health");

            // Only health checks tagged with the "live" tag must pass for app to be considered alive
            app.MapHealthChecks("/alive", new HealthCheckOptions
            {
                Predicate = r => r.Tags.Contains("live")
            });
        }

        return app;
    }

    //private static void AddEventBusSubscriptions(this IEventBusBuilder eventBus)
    //{
    //    eventBus.AddSubscription<ProductPriceChangedIntegrationEvent, ProductPriceChangedIntegrationEventHandler>();
    //    eventBus.AddSubscription<OrderStatusChangedToShippedIntegrationEvent, OrderStatusChangedToShippedIntegrationEventHandler>();
    //    eventBus.AddSubscription<OrderStatusChangedToPaidIntegrationEvent, OrderStatusChangedToPaidIntegrationEventHandler>();
    //}
}
