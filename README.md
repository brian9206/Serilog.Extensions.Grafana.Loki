# Serilog.Extensions.Grafana.Loki
A pointless wrapper to set up `Serilog.Sinks.Grafana.Loki` with optimal settings in few lines.

## Usage
`Startup.cs` or `Program.cs` (.NET 6)
```csharp
using Serilog.Sinks.Grafana.Loki;

.UseSerilog((hostingContext, logger) =>
{
    logger.WriteTo.GrafanaLokiCommonSettings(hostingContext.Configuration, hostingContext.Environment);
})
```

`appsettings.json`
```json
{
  "Serilog": {
    "Using":  [ "Serilog.Sinks.Console" ],
    "MinimumLevel": "Verbose",
    "WriteTo": [
      { "Name": "Console" }
    ],
    "Enrich": [ "FromLogContext", "WithMachineName" ]
  },
  "LokiConfig": {
    "Url": "https://my.grafana.loki.server",
    "Credentials": {
      "Login": "username",
      "Password": "password"
    }
  }
}
```

Example log
```csharp
_logger.LogInformation("This is a test {time}", DateTimeOffset.Now);
```

and you will see this in Grafana
```
[00:17:39 INF] This is a test "2022-06-05T00:17:39.5481985+08:00"
time="06/05/2022 00:17:39 +08:00" SourceContext="WorkerService1.Worker" 

Log labels:
app = WorkerService1
env = Development
host = YOUR_HOST_NAME
level = info

Detected fields:
SourceContext = "WorkerService1.Worker"
time = "06/05/2022 00:17:39 +08:00"
ts = ...
tsNs = ...
```

## Additional Labels
`appsettings.json`
```json
{
  "LokiConfig": {
    "Url": "",
    "Credentials": {
      "Login": "",
      "Password": ""
    },
    "Labels": {
      "some_key1": "some_value1",
      "some_key2": "some_value2"
    }
  }
}
```
```
LokiConfig__Labels__some_key1=some_value1
LokiConfig__Labels__some_key2=some_value2
```

or you can add `service` label in code level in `Startup.cs` or `Program.cs`
```csharp
using Serilog.Sinks.Grafana.Loki;

.UseSerilog((hostingContext, logger) =>
{
    logger.WriteTo.GrafanaLokiCommonSettings(hostingContext.Configuration, hostingContext.Environment, "my service name");
})
```

## Full settings
See `LokiConfig.cs`
```json
{
  "LokiConfig": {
    "Url": "",
    "Credentials": {
      "Login": "",
      "Password": ""
    },
    "Labels": {
    },
    "RestrictedToMinimumLevel": "Verbose",
    "BatchPostingLimit": 1000,
    "QueueLimit": null,
    "Period": null
  }
}
```
```
LokiConfig__Url=
LokiConfig__Credentials__Login=
LokiConfig__Credentials__Password=
LokiConfig__RestrictedToMinimumLevel=Verbose
LokiConfig__BatchPostingLimit=1000
LokiConfig__QueueLimit=null
LokiConfig__Period=null
```

## License
MIT