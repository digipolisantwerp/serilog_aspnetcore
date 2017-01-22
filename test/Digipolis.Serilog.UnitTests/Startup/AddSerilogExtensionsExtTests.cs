using System;
using System.Linq;
using Digipolis.Serilog.Enrichers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Serilog.Core;
using Xunit;

namespace Digipolis.Serilog.UnitTests.Startup
{
    public class AddSerilogExtensionsExtTests
    {
        [Fact]
        void OptionsNullRaisesArgumentNullException()
        {
            var services = new ServiceCollection();
            var ex = Assert.Throws<ArgumentNullException>(() => services.AddSerilogExtensions(null));
            Assert.Equal("setupAction", ex.ParamName);
            Assert.Contains("setupAction can not be null.", ex.Message);
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

        [Fact]
        void EnricherIsRegisteredAsSingleton()
        {
            var services = new ServiceCollection();
            services.AddSerilogExtensions(options => {
                options.MessageVersion = "2";
                options.AddMessagEnricher();
            });

            var registrations = services.Where(sd => sd.ServiceType == typeof(ILogEventEnricher) &&
                                                     sd.ImplementationType == typeof(MessageEnricher))
                                                     .ToArray();

            Assert.Equal(1, registrations.Count());
            Assert.Equal(ServiceLifetime.Singleton, registrations[0].Lifetime);
        }

        [Fact]
        void IHttpContextAccessorIsRegisteredAsSingleton()
        {
            var services = new ServiceCollection();
            services.AddSerilogExtensions(options => {
                options.MessageVersion = "2";
            });

            var registrations = services.Where(sd => sd.ServiceType == typeof(IHttpContextAccessor) &&
                                                     sd.ImplementationType == typeof(HttpContextAccessor))
                                                     .ToArray();

            Assert.Equal(1, registrations.Count());
            Assert.Equal(ServiceLifetime.Singleton, registrations[0].Lifetime);
        }

        [Fact]
        void ApplicationLoggerIsRegisteredAsSingleton()
        {
            var services = new ServiceCollection();
            services.AddSerilogExtensions(options => {
                options.MessageVersion = "2";
                options.EnableApplicationLogger = true;
            });

            var registrations = services.Where(sd => sd.ServiceType == typeof(IApplicationLogger) &&
                                                     sd.ImplementationType == typeof(ApplicationLogger))
                                                     .ToArray();

            Assert.Equal(1, registrations.Count());
            Assert.Equal(ServiceLifetime.Singleton, registrations[0].Lifetime);
        }
    }
}
