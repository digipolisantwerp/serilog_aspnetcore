using System;
using Microsoft.Extensions.Options;
using Serilog.Core;
using Serilog.Events;

namespace Digipolis.Serilog.Enrichers
{
    public class MessageEnricher : ILogEventEnricher
    {
        public MessageEnricher(IOptions<SerilogExtensionsOptions> options)
        {
            _options = options.Value;
        }

        private readonly SerilogExtensionsOptions _options;

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty(LoggingProperties.MessageVersion, _options.MessageVersion ?? LoggingProperties.NullValue));
        }
    }
}
