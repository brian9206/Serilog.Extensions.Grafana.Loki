using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog.Configuration;
using Serilog.Sinks.Grafana.Loki;
using Serilog.Sinks.Grafana.Loki.HttpClients;

namespace Serilog.Extensions.Grafana.Loki
{
    public static class LoggerSettingsConfigurationExtensions
    {
        public static LoggerSinkConfiguration GrafanaLokiCommonSettings(
            this LoggerSinkConfiguration loggerSettings,
            IConfiguration configuration,
            IHostEnvironment environment,
            string service = null)
        {
            var lokiConfigSection = configuration.GetSection("LokiConfig");
            
            if (lokiConfigSection != null && lokiConfigSection.Exists())
            {
                var lokiConfig = new LokiConfig();
                lokiConfigSection.Bind(lokiConfig);

                loggerSettings.GrafanaLoki(lokiConfig.Url, 
                    httpClient: new LokiGzipHttpClient(),
                    credentials: lokiConfig.Credentials,
                    labels: lokiConfig.GetLabels(environment, service),
                    queueLimit: lokiConfig.QueueLimit,
                    period: lokiConfig.Period,
                    batchPostingLimit: lokiConfig.BatchPostingLimit,
                    restrictedToMinimumLevel: lokiConfig.RestrictedToMinimumLevel,
                    createLevelLabel: true,
                    filtrationMode: LokiLabelFiltrationMode.Include,
                    textFormatter: new AppendPropertiesTextFormatter());
            }

            return loggerSettings;
        }
    }
}