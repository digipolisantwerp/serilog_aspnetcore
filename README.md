# Serilog extensions library

Digipolis Antwerp uses the Elastic stack for logging. Instead of writing our own framework for this logging engine, we use the excellent [Serilog](https://serilog.net/) library.  
Our library adds extensions to the Serilog framework, specific to the way we use the Elastic stack.

## Table of Contents

<!-- START doctoc generated TOC please keep comment here to allow auto update -->
<!-- DON'T EDIT THIS SECTION, INSTEAD RE-RUN doctoc TO UPDATE -->


- [Target framework](#target-framework)
- [Installation](#installation)
- [Usage](#usage)
- [Extending](#extending)
- [Breaking changes in version 2](#breaking-changes-in-version-2)
- [Enrichment extension packages](#enrichment-extension-packages)

<!-- END doctoc generated TOC please keep comment here to allow auto update -->

## Target framework

This package targets **.NET Standard 2.0**.

For .NET Standard 1.6, use version 3.x. The source code is available in [this legacy branch](https://github.com/digipolisantwerp/serilog_aspnetcore/tree/legacy-3.x).

## Installation

To add the library to a project, you add the package to the csproj file :

```xml
  <ItemGroup>
    <PackageReference Include="Digipolis.Serilog" Version="4.0.0" />
  </ItemGroup>
``` 

In Visual Studio you can also use the NuGet Package Manager to do this.

## Usage

This library serves as the base library for the more concrete Serilog extension libraries in the Digipolis framework. It contains the extension method to register the 
Digipolis Serilog extensions, to be called in the **ConfigureServices** method of the **Startup** class.

```csharp  
services.AddSerilogExtensions(options => {
    // call the specific extension here
});
```  

## Extending

To extend the logging framework, you create a concrete package that contains the extensions e.g. a new LogEvent enricher.  
To register the extensions, you also add an **extension method** to the **SerilogExtensionsOptions** class, like so :

```csharp  
public static SerilogExtensionsOptions AddApplicationServicesEnricher(this SerilogExtensionsOptions options)
{
    options.ApplicationServices.AddSingleton<ILogEventEnricher, ApplicationServicesEnricher>();
    return options;
}
```  

In an application, your extension is registered in the **ConfigureServices** method of the **Startup** class :  

```csharp  
services.AddSerilogExtensions(options => {
    options.AddApplicationServicesEnricher();
});
```  

This way, your enrichers can have other services injected into them at runtime by the .NET Core injection framework.

Registered enrichers are added to the Serilog pipeline in the **Configure** method of the **Startup** class when configuring the Serilog logging framework : 

```csharp  
var enrichers = app.ApplicationServices.GetServices<ILogEventEnricher>().ToArray();

Log.Logger = new LoggerConfiguration()
                .Enrich.With(enrichers)
                .WriteTo.LiterateConsole()
                .CreateLogger();

loggerFactory.AddSerilog(dispose: true);
```  

## Breaking changes in version 2

If you upgrade from version 1.x to version 2.x there are some changes you have to make in your project.

Some of the extensions that this package added to the Serilog Elastic Sink are now part of the official Serilog package(s) and were thus removed from this library. The consequence is that you don't get the implicit reference to the Serilog packages anymore when you add this package to your project.  **You have to add the Serilog packages to your own csproj project file** :

```xml
  <ItemGroup>
    <PackageReference Include="Digipolis.Serilog" Version="4.0.0" />
    <PackageReference Include="Digipolis.Serilog.ApplicationServices" Version="3.0.0" />
    <PackageReference Include="Digipolis.Serilog.AuthService" Version="3.0.0" />
    <PackageReference Include="Digipolis.Serilog.Correlation" Version="3.0.0" />
    <PackageReference Include="Digipolis.Serilog.Message" Version="1.0.0" />
    <PackageReference Include="Serilog.Settings.Configuration" Version="3.0.0" />
    <PackageReference Include="Serilog.Sinks.Elasticsearch" Version="5.0.0" />
  </ItemGroup>
``` 

If you were using the IApplicationLogger in your project, you will now have to provide one yourself, since it has been removed from this version of the Digipolis.Serilog package (it has been moved to our [ASP.NET Core API project generator](https://github.com/digipolisantwerp/generator-dgp-api-aspnetcore_yeoman)).

Here's the one that was included in the previous version : 

```csharp
public interface IApplicationLogger : ILogger<ApplicationLogger>
{ }

public class ApplicationLogger : IApplicationLogger
{
    public ApplicationLogger(ILogger<ApplicationLogger> logger)
    {
        _logger = logger;
    }

    private readonly ILogger<ApplicationLogger> _logger;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
        _logger.Log(logLevel, eventId, state, exception, formatter);
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return _logger.IsEnabled(logLevel);
    }

    public IDisposable BeginScope<TState>(TState state)
    {
        return _logger.BeginScope(state);
    }
}
```  

Don't forget to register it in the .NET Core DI container if you want to inject it in your classes : 

```csharp 
services.AddSingleton<IApplicationLogger, ApplicationLogger>();
```  

In the Digipolis architecture, 2 types of events are used while logging : system-logevents and application-logevents.  
System-logevents are meant for system administrators and developers that want to diagnose problems with the application. They can contain stacktraces and other internal information. They will not be visible for normal users of the application.  
Application-logevents are used for functional logging like for example completed steps in a business flow. In most applications, these log-events will also be shown to a user of the application so can not contain technical details.  

Together with the following example configuration, a developer can easily use ILogger<T> to send system-logevents and IApplicationLogger to send application-logevents to Elasticsearch.

**loggingconfig.json** :

```json
{
  "SystemLog": {
    "WriteTo": [
    {
      "Name": "Elasticsearch",
      "Args": {
        "nodeUris": "http://localhost:9200",
        "indexFormat": "logstash-myapp-{0:yyyy.MM.dd}",
        "templateName": "myapp-template",
        "typeName": "SystemLogEvent",
        "restrictedToMinimumLevel": "Debug"
        }
      }],
      "Enrich": [ "FromLogContext" ]
	},
    "ApplicationLog": {
      "WriteTo": [
      {
        "Name": "Elasticsearch",
        "Args": {
          "nodeUris": "http://localhost:9200",
          "indexFormat": "logstash-myapp-{0:yyyy.MM.dd}",
          "templateName": "myapp-template",
          "typeName": "AppLogEvent",
          "restrictedToMinimumLevel": "Information"
        }
      }],
      "Enrich": [ "FromLogContext" ]
  }
}
```  

**Startup.Configure** :

```csharp  
var enrichers = app.ApplicationServices.GetServices<ILogEventEnricher>().ToArray();

var systemLogSection = Configuration.GetSection("SystemLog");
var applicationLogSection = Configuration.GetSection("ApplicationLog");

var appLogger = typeof(ApplicationLogger).FullName;

Log.Logger = new LoggerConfiguration()
                .Enrich.With(enrichers)
                .WriteTo.Logger(l => l.ReadFrom.ConfigurationSection(systemLogSection).Filter.ByExcluding(Matching.FromSource(appLogger)))
                .WriteTo.Logger(l => l.ReadFrom.ConfigurationSection(applicationLogSection).Filter.ByIncludingOnly(Matching.FromSource(appLogger)))
                .CreateLogger();

loggerFactory.AddSerilog(dispose: true);

appLifetime.ApplicationStopped.Register(Log.CloseAndFlush);
```  

You can find a more detailed example in our ASP.NET Core API project generator : https://github.com/digipolisantwerp/generator-dgp-api-aspnetcore_yeoman.

The **MessageEnricher** has been moved to its own package. If you want to use it, you will have to add the package to your csproj file (example is higher up in this chapter).  
Consequently the MessageVersion option has been moved and is now configured with the following code : 

```csharp  
services.AddSerilogExtensions(options => {
                options.AddMessagEnricher(msgOptions => msgOptions.MessageVersion = "1");
            });
```  

## Enrichment extension packages

[Digipolis.Serilog.ApplicationServices](https://github.com/digipolisantwerp/serilog-applicationservices_aspnetcore)  
[Digipolis.Serilog.AuthService](https://github.com/digipolisantwerp/serilog-authservice_aspnetcore)  
[Digipolis.Serilog.Correlation](https://github.com/digipolisantwerp/serilog-correlation_aspnetcore)  
[Digipolis.Serilog.Message](https://github.com/digipolisantwerp/serilog-message_aspnetcore)  

## Contributing

Pull requests are always welcome, however keep the following things in mind:

- New features (both breaking and non-breaking) should always be discussed with the [repo's owner](#support). If possible, please open an issue first to discuss what you would like to change.
- Fork this repo and issue your fix or new feature via a pull request.
- Please make sure to update tests as appropriate. Also check possible linting errors and update the CHANGELOG if applicable.

## Support

Peter Brion (<peter.brion@digipolis.be>)
