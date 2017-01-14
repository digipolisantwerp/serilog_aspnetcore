using System;
using Digipolis.Serilog.Enrichers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace Digipolis.Serilog.UnitTests.Enrichers
{
    public class MessageEnricherTests
    {
        [Fact]
        void VersionIsSetFromOptions()
        {
            var services = new ServiceCollection();
            services.AddOptions();
            services.Configure<SerilogExtensionsOptions>(opt => opt.MessageVersion = "5");

            var serviceProvider = services.BuildServiceProvider();
            var options = serviceProvider.GetRequiredService<IOptions<SerilogExtensionsOptions>>();
            var enricher = new MessageEnricher(options);

            Assert.Equal("5", enricher.MessageVersion);
        }
    }
}
