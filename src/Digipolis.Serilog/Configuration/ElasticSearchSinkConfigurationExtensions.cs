using System;
using System.Collections.Generic;
using System.Linq;
using Serilog.Configuration;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;

namespace Serilog
{
    public static class ElasticSearchSinkConfigurationExtensions
    {
        public static LoggerConfiguration Elasticsearch(
            this LoggerSinkConfiguration loggerSinkConfiguration,
            string nodeUris,
            string indexFormat = null,
            string templateName = null,
            string typeName = null,
            int batchPostingLimit = 50,
            double period = 2000,
            bool inlineFields = false,
            string bufferBaseFilename = null,
            long? bufferFileSizeLimitBytes = null,
            double? bufferLogShippingInterval = null,
            LogEventLevel minimumLogEventLevel = LogEventLevel.Information)
        {
            if ( string.IsNullOrEmpty(nodeUris) ) throw new ArgumentNullException("nodeUris", "No Elasticsearch node(s) specified.");

            IEnumerable<Uri> nodes = nodeUris
                .Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(uriString => new Uri(uriString));

            var options = new ElasticsearchSinkOptions(nodes);

            if ( !string.IsNullOrWhiteSpace(indexFormat) )
            {
                options.IndexFormat = indexFormat;
            }

            if ( !string.IsNullOrWhiteSpace(templateName) )
            {
                options.AutoRegisterTemplate = true;
                options.TemplateName = templateName;
            }

            if ( !string.IsNullOrWhiteSpace(typeName) )
            {
                options.TypeName = typeName;
            }

            if ( !string.IsNullOrWhiteSpace(bufferBaseFilename) )
            {
                options.BufferBaseFilename = bufferBaseFilename;
                
            }

            if ( bufferFileSizeLimitBytes.HasValue )
            {
                options.BufferFileSizeLimitBytes = bufferFileSizeLimitBytes;
            }

            if ( bufferLogShippingInterval.HasValue )
            {
                options.BufferLogShippingInterval = TimeSpan.FromMilliseconds(bufferLogShippingInterval.Value);
            }

            options.Period = TimeSpan.FromMilliseconds(period);
            options.BatchPostingLimit = batchPostingLimit;
            options.InlineFields = inlineFields;
            options.MinimumLogEventLevel = minimumLogEventLevel;

            return loggerSinkConfiguration.Elasticsearch(options);
        }
    }
}
