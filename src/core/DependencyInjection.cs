using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace TaskProcessor.Core;
public static class DependencyInjection
{
    public static IServiceCollection AddCoreApplication(this IServiceCollection service, IConfiguration configuration)
    {
        return service;
    }
}