using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Hosting;
using Serilog.Events;
using Serilog.Sinks.Grafana.Loki;

namespace Serilog.Extensions.Grafana.Loki
{
    public class LokiConfig
    {
        public string Url { get; set; } = string.Empty;
        public LokiCredentials Credentials { get; set; }
        public Dictionary<string, string> Labels { get; set; } = new Dictionary<string, string>();
        public LogEventLevel RestrictedToMinimumLevel { get; set; } = LevelAlias.Minimum;
        public int BatchPostingLimit { get; set; } = 1000;
        public int? QueueLimit { get; set; } = null;
        public TimeSpan? Period { get; set; } = null;
        
        public IEnumerable<LokiLabel> GetLabels(IHostEnvironment env, string service = null)
        {
            var labels = new List<LokiLabel>()
            {
                new LokiLabel()
                {
                    Key = "app",
                    Value = env.ApplicationName
                },
                new LokiLabel()
                {
                    Key = "host",
                    Value = Environment.MachineName
                },
                new LokiLabel()
                {
                    Key = "env",
                    Value = env.EnvironmentName
                }
            };

            if (!string.IsNullOrEmpty(service))
            {
                labels.Add(new LokiLabel()
                {
                    Key = "service",
                    Value = service
                });
            }
        
            labels.AddRange(Labels.Select(label => new LokiLabel()
            {
                Key = label.Key,
                Value = label.Value
            }));

            return labels;
        }
    }
}