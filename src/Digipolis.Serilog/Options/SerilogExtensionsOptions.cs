using System;
using Microsoft.Extensions.DependencyInjection;

namespace Digipolis.Serilog
{
    public class SerilogExtensionsOptions
    {
        public SerilogExtensionsOptions(IServiceCollection services)
        {
            if ( services == null ) throw new ArgumentNullException(nameof(services), $"{nameof(services)} is null.");

            ApplicationServices = services;
        }

        public  IServiceCollection ApplicationServices { get; private set; }
    }
}
