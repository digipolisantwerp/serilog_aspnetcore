using System;
using Digipolis.Serilog.Enrichers;

namespace Digipolis.Serilog
{
    public static class AddMessageEnricherExt
    {
        public static SerilogExtensionsOptions AddMessagEnricher(this SerilogExtensionsOptions options)
        {
            options.AddEnricher<MessageEnricher>();
            return options;
        }
    }
}
