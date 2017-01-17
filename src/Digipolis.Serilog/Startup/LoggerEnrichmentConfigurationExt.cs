using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Configuration;
using Serilog.Core;

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
