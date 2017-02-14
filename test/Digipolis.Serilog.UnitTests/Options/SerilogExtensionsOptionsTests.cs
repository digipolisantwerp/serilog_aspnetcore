using System;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Digipolis.Serilog.UnitTests.Options
{
    public class SerilogExtensionsOptionsTests
    {
        [Fact]
        void ServicesNullRaisesArgumentNullException()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => new SerilogExtensionsOptions(null));
            Assert.Equal("services", ex.ParamName);
            Assert.Contains("services is null", ex.Message);
        }

        [Fact]
        void ApplicationServicesIsSet()
        {
            var services = new ServiceCollection();
            var options = new SerilogExtensionsOptions(services);
            Assert.Same(services, options.ApplicationServices);
        }
    }
}
