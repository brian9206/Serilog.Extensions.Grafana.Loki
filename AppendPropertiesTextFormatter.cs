using System.Collections.Generic;
using System.IO;
using System.Linq;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Formatting.Display;

namespace Serilog.Extensions.Grafana.Loki
{
    public class AppendPropertiesTextFormatter : ITextFormatter
    {
        private readonly ITextFormatter _formatter;
        private readonly IReadOnlyCollection<string> _ignorePropertyNames;

        public AppendPropertiesTextFormatter(ITextFormatter formatter = null, IEnumerable<string> ignorePropertyNames = null)
        {
            _formatter = formatter ?? new MessageTemplateTextFormatter("[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}");
            _ignorePropertyNames = ignorePropertyNames?.ToList() ?? (IReadOnlyCollection<string>) new [] { "MachineName", "EnvironmentName", "envName" };
        }

        public void Format(LogEvent logEvent, TextWriter output)
        {
            _formatter.Format(logEvent, output);

            if (logEvent.Properties == null || !logEvent.Properties.Keys.Any()) 
                return;

            var last = logEvent.Properties.LastOrDefault();
            foreach (var property in logEvent.Properties)
            {
                if (_ignorePropertyNames.Contains(property.Key))
                    continue;
            
                output.Write($"{property.Key}=");
            
                var value = property.Value.ToString();
                if (!value.StartsWith("\"") && !value.EndsWith("\"") && value.Contains(' '))
                    value = $"\"{value.Replace("\"", "\\\"")}\"";

                output.Write(value);
            
                if (!property.Equals(last))
                    output.Write(" ");
            }
        }
    }
}