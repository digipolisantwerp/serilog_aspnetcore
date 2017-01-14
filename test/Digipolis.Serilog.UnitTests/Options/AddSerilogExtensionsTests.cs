using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace Digipolis.Serilog.UnitTests.Options
{
    public class AddSerilogExtensionsTests
    {
        [Fact]
        void OptionsNullRaisesArgumentNullException()
        {
            var services = new ServiceCollection();
            var ex = Assert.Throws<ArgumentNullException>(() => services.AddSerilogExtensions(null));
            Assert.Equal("setupAction", ex.ParamName);
            Assert.Contains("SerilogExtensionsOptions can not be null.", ex.Message);
        }

        [Fact]
        void OptionsAreRegisteredAsSingleton()
        {
            var services = new ServiceCollection();
            services.AddSerilogExtensions(options => options.MessageVersion = "3");

            var registrations = services.Where(sd => sd.ServiceType == typeof(IConfigureOptions<SerilogExtensionsOptions>))
                                        .ToArray();
            Assert.Equal(1, registrations.Count());
            Assert.Equal(ServiceLifetime.Singleton, registrations[0].Lifetime);
        }
    }
}
