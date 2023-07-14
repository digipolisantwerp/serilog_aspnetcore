using Microsoft.Extensions.DependencyInjection;
using System;

namespace Digipolis.Serilog
{
    public static class AddSerilogExtensionsExt
    {
        public static IServiceCollection AddSerilogExtensions(this IServiceCollection services, Action<SerilogExtensionsOptions> setupAction)
        {
            if ( setupAction == null ) throw new ArgumentNullException(nameof(setupAction), $"{nameof(setupAction)} can not be null.");

            var options = new SerilogExtensionsOptions(services);
            setupAction.Invoke(options);

            return services;
        }
    }
}
