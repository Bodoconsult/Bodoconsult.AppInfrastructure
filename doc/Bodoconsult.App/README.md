Bodoconsult.App
================

# Overview

## What does the library

Bodoconsult.App is a library with basic functionality for multilayered monolithic applications like database based client server apps. 
It delivers the following main functionality:

> [Dependency injection container DiContainer](#dependency-injection-container-dicontainer)

> [App start infrastructure for diverse app types](#app-start-infrastructure-for-diverse-app-types)

> [Single-threaded high-performance logging for single-threaded or multi-threaded environments (IAppLoggerProxy / AppLoggerProxy )](#logging-with-iapploggerproxy--apploggerproxy)

> [Application performance measurement (APM) tools for performnce logging](#performance-logging)

> [Business transactions to simplify transportation layer implementations for technologies like GRPC, WebAPI, etc...](#business-transactions)

> [IDataExportService<T>/DataExportServiceBase<T> for long running data exports to string or binary files](#data-export-services)

> [# Client notifications for technologies like GRPC, WebAPI, etc...](#client-notifications)

> [More tools for developers](#more-tools-for-developers)


## Prerequisites

If you get an exception "Could not load file or assembly 'Microsoft.Extensions.Logging.Console, Version=9.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60'. ..." you need to add the following packages to your app.

``` xml
    <PackageReference Include="log4net" Version="3.2.0" />
    <PackageReference Include="Microsoft.Extensions.Configuration.Json" Version="9.0.11" />
    <PackageReference Include="Microsoft.Extensions.DependencyInjection" Version="9.0.11" />
    <PackageReference Include="Microsoft.Extensions.Logging" Version="9.0.11" />
    <PackageReference Include="Microsoft.Extensions.Logging.Console" Version="9.0.11" />
    <PackageReference Include="Microsoft.Extensions.Logging.Debug" Version="9.0.11" />
```	

During JSON deserialization of appsettings.json this packages are required.

## How to use the library

The source code contains NUnit test classes the following source code is extracted from. The samples below show the most helpful use cases for the library.

# Dependency injection container DiContainer

A dependency container is a piece of software for simpler handling of dependencies between classes in the code of an app.

See page [Dependency injection container DiContainer](di.md) for details.

# App start infrastructure for diverse app types

Bodoconsult.App contains the infrastructure to set up certain types of apps with commonly used features like

-   Reading appsettings.json, keep it in memory for later usage and extract connection string and logging settings from it
-   Setup a central logger
-   Setup DI containers for production and testing environment
-   Start the console app and run workload in a separate thread
-   Unhandled exception handling

Bodoconsult.App currently supports the following MS Windows apps:

-   Console app
-   WinForms apps (classical WinForms app or service-like app with a very limited WinForms based UI)
-   WPF (classical WPF app or service-like app with a very limited WPF based UI)
-   Avalonia (classical Avalonia app or service-like app with a very limited Avalonia based UI)
-   Windows background service
-   Windows background service hosting a GRPC server service

See page [app start infrastructure](AppStartInfrastructure.md) for details.

# Logging with IAppLoggerProxy / AppLoggerProxy

AppLoggerProxy is a high performance logger infrastructure for multithreaded and or multitasked apps with low resulting garbage pressure.

See page [Logging with IAppLoggerProxy / AppLoggerProxy](Logging.md) for details

# Performance logging

Performance logging infrastructure is implemented based on the https://learn.microsoft.com/en-us/dotnet/core/diagnostics/. 
So it can be used with professionell APM tools like Application Insights, dotnet-counters, and dotnet-monitor.

See page [Performance logging](PerformanceLogging.md) for details.

# Business transactions

See page [Business transactions](BusinessTransactions.md) for details.

# Client notifications

See page [Client notifications](ClientNotification.md) for details.

# Data export services

Data export services based on IDataExportService<T>/DataExportServiceBase<T> are intended to store a datastream of messages like raw byte arrays, strings, custom data classes etc. to file.

Typical scenario is a network device sending communication data to a client. Client is intended to save the received data into files for later usage.

See page [](DataExportService.md) for details.

# More tools for developers

Bodoconsult.App provides more tools like WatchDog, ProducerConsumerQueue, BufferPool etc.

See page [More tools for developers](Others.md) for details.

# About us

Bodoconsult <http://www.bodoconsult.de> is a Munich based software company from Germany.

Robert Leisner is senior software developer at Bodoconsult. See his profile on <http://www.bodoconsult.de/Curriculum_vitae_Robert_Leisner.pdf>.

