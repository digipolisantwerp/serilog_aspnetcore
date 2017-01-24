using System;
using Microsoft.Extensions.Logging;

namespace Digipolis.Logging
{
    public interface IApplicationLogger : ILogger<ApplicationLogger>
    { }
}
