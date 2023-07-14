using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using System.Linq;

namespace Digipolis.Serilog.Startup
{
    public static class LoggerEnrichmentConfigurationExt
    {
        public static LoggerConfiguration WithRegisteredEnrichers(this LoggerEnrichmentConfiguration config, IApplicationBuilder app)
        {
            var enrichers = app.ApplicationServices.GetServices<ILogEventEnricher>().ToArray();
            return config.With(enrichers);
        }
    }
}
