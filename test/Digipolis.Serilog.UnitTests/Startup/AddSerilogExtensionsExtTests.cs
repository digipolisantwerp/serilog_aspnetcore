using System;
using Microsoft.Extensions.DependencyInjection;
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
        void setupActionIsInvoked()
        {
            var isInvoked = false;

            var services = new ServiceCollection();
            services.AddSerilogExtensions(options => isInvoked = true);
            Assert.True(isInvoked);
        }
    }
}
