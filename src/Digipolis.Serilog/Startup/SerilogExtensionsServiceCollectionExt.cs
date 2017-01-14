using System;
using Microsoft.Extensions.DependencyInjection;
using Serilog.Core;

namespace Digipolis.Serilog
{
    public static class SerilogExtensionsServiceCollectionExt
    {
        public static IServiceCollection AddSerilogExtensions(this IServiceCollection services, Action<SerilogExtensionsOptions> setupAction)
        {
            if ( setupAction == null ) throw new ArgumentNullException(nameof(setupAction), $"{nameof(setupAction)} can not be null.");

            services.Configure(setupAction);

            var options = new SerilogExtensionsOptions();
            setupAction.Invoke(options);

            foreach ( var type in options.EnricherTypes )
            {
                services.AddSingleton(typeof(ILogEventEnricher), type);
            }

            return services;
        }
    }
}
