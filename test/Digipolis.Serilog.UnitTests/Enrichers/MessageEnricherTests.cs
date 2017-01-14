using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Digipolis.Serilog.UnitTests.Enrichers
{
    public class MessageEnricherTests
    {
        [Fact]
        void VersionIsSetFromOptions()
        {
            var services = new ServiceCollection();
            services.Configure<SerilogExtensionsOptions>(options => options.MessageVersion = "5");
            var serviceProvider = services.BuildServiceProvider();



        }
    }
}
