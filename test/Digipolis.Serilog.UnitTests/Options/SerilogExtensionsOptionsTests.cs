using System;
using System.Linq;
using Digipolis.Serilog.Enrichers;
using Xunit;

namespace Digipolis.Serilog.UnitTests.Options
{
    public class SerilogExtensionsOptionsTests
    {
        [Fact]
        void MessageVersionIsInitialized()
        {
            var options = new SerilogExtensionsOptions();
            Assert.Equal(SerilogExtensionsDefaults.MessageVersion, options.MessageVersion);
        }

        [Fact]
        void EnricherTypesIsInitialized()
        {
            var options = new SerilogExtensionsOptions();
            Assert.NotNull(options.EnricherTypes);
            Assert.Equal(0, options.EnricherTypes.Count());
        }

        [Fact]
        void EnricherTypeIsAdded()
        {
            var options = new SerilogExtensionsOptions();
            options.AddEnricher<MessageEnricher>();
            Assert.Collection(options.EnricherTypes, (item) => Assert.Equal(typeof(MessageEnricher), item));
        }
    }
}
