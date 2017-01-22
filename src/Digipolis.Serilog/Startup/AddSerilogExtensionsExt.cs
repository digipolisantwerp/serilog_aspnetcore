using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Serilog.Core;

namespace Digipolis.Serilog
{
    public static class AddSerilogExtensionsExt
    {
        public static IServiceCollection AddSerilogExtensions(this IServiceCollection services, Action<SerilogExtensionsOptions> setupAction)
        {
            if ( setupAction == null ) throw new ArgumentNullException(nameof(setupAction), $"{nameof(setupAction)} can not be null.");

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.Configure(setupAction);

            var options = new SerilogExtensionsOptions();
            setupAction.Invoke(options);

            if ( options.EnableApplicationLogger ) services.AddSingleton<IApplicationLogger, ApplicationLogger>();

            foreach ( var type in options.EnricherTypes )
            {
                services.AddSingleton(typeof(ILogEventEnricher), type);
            }

            return services;
        }
    }
}
