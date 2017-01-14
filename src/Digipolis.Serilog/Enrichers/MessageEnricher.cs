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
            MessageVersion = options.Value?.MessageVersion ?? LoggingProperties.NullValue;
        }

        public string MessageVersion { get; private set; }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty(LoggingProperties.MessageVersion, MessageVersion));
        }
    }
}
