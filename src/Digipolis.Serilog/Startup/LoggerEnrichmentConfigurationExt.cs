using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using System;
using System.Linq;

namespace Digipolis.Serilog.Startup
{
    public static class LoggerEnrichmentConfigurationExt
    {
        // 20230714 obsolete, use version with IServiceProvider
        [Obsolete]
        public static LoggerConfiguration WithRegisteredEnrichers(this LoggerEnrichmentConfiguration config, IApplicationBuilder app)
        {
            var enrichers = app.ApplicationServices.GetServices<ILogEventEnricher>().ToArray();
            return config.With(enrichers);
        }

        public static LoggerConfiguration WithRegisteredEnrichers(this LoggerEnrichmentConfiguration config, IServiceProvider serviceProvider)
        {
            var enrichers = serviceProvider.GetServices<ILogEventEnricher>().ToArray();
            return config.With(enrichers);
        }
    }
}
