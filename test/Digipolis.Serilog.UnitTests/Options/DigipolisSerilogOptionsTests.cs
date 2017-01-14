using System;
using Xunit;

namespace Digipolis.Serilog.UnitTests.Options
{
    public class DigipolisSerilogOptionsTests
    {
        [Fact]
        void MessageVersionIsDefaulted()
        {
            var options = new SerilogExtensionsOptions();
            Assert.Equal(SerilogExtensionsDefaults.MessageVersion, options.MessageVersion);
        }
    }
}
