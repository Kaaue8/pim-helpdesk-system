using HelpDesk.Api.Services;

namespace HelpDesk.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<AuthService>();
        services.AddScoped<HoustonService>();

        // Aqui no futuro:
        // services.AddScoped<TicketService>();
        // services.AddScoped<UserService>();
        // services.AddScoped<IAService>();

        return services;
    }
}
