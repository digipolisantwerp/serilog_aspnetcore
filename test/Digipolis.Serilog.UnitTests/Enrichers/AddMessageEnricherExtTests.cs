using System;
using Digipolis.Serilog.Enrichers;
using Xunit;

namespace Digipolis.Serilog.UnitTests.Enrichers
{
    public class AddMessageEnricherExtTests
    {
        [Fact]
        void MessageEnricherTypeIsAdded()
        {
            var options = new SerilogExtensionsOptions();
            options.AddMessagEnricher();
            Assert.Collection(options.EnricherTypes, (item) => Assert.Equal(typeof(MessageEnricher), item));
        }
    }
}
