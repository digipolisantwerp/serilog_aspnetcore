using System;
using Microsoft.Extensions.Logging;

namespace Digipolis.Serilog
{
    public interface IApplicationLogger : ILogger<ApplicationLogger>
    { }
}
