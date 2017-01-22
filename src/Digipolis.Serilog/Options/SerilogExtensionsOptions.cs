using System;
using System.Collections.Generic;
using Serilog.Core;

namespace Digipolis.Serilog
{
    public class SerilogExtensionsOptions
    {
        public string MessageVersion { get; set; } = SerilogExtensionsDefaults.MessageVersion;

        public bool EnableApplicationLogger { get; set; }

        readonly List<Type> _enricherTypes = new List<Type>();
        public IEnumerable<Type> EnricherTypes
        {
            get { return _enricherTypes; }
        }

        public void AddEnricher<TType>() where TType : ILogEventEnricher
        {
            _enricherTypes.Add(typeof(TType));
        }
    }
}
