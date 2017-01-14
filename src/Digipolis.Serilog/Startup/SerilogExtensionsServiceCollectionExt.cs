using System;
using Microsoft.Extensions.DependencyInjection;

namespace Digipolis.Serilog
{
    public static class SerilogExtensionsServiceCollectionExt
    {
        public static IServiceCollection AddSerilogExtensions(this IServiceCollection services, Action<SerilogExtensionsOptions> setupAction)
        {
            


            return services;
        }
    }
}
