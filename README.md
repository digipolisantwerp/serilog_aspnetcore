# Serilog extensions library

Extensions for the Serilog logging framework.

## Table of Contents

<!-- START doctoc generated TOC please keep comment here to allow auto update -->
<!-- DON'T EDIT THIS SECTION, INSTEAD RE-RUN doctoc TO UPDATE -->

- [Installation](#installation)

<!-- END doctoc generated TOC please keep comment here to allow auto update -->

## Installation

To add the library to a project, you add the package to the project.json :

``` json 
"dependencies": {
    "Digipolis.Serilog":  "2.0.1"
 }
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

This way, your enrichers can have other services injected into them at runtime.

Registered enrichers are added to the Serilog pipeline in the **Configure** method of the **Startup** class when configuring the Serilog logging framework : 

```csharp  
var enrichers = app.ApplicationServices.GetServices<ILogEventEnricher>().ToArray();

Log.Logger = new LoggerConfiguration()
                .Enrich.With(enrichers)
                .WriteTo.LiterateConsole()
                .CreateLogger();

loggerFactory.AddSerilog(dispose: true);
```  

## Already existing extension packages

[Digipolis.Serilog.ApplicationServices](https://github.com/digipolisantwerp/serilog-applicationservices_aspnetcore)  
[Digipolis.Serilog.AuthService](https://github.com/digipolisantwerp/serilog-authservice_aspnetcore)  
[Digipolis.Serilog.Correlation](https://github.com/digipolisantwerp/serilog-correlation_aspnetcore)  
[Digipolis.Serilog.Message](https://github.com/digipolisantwerp/serilog-message_aspnetcore)  
