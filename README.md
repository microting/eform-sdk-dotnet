# eform-sdk-dotnet ![C# Build status](https://microtingas2017.visualstudio.com/_apis/public/build/definitions/5f551ab2-01ab-4204-8efa-06be93328bc1/1/badge)

[![NuGet Badge](https://buildstats.info/nuget/Microting.eForm)](https://www.nuget.org/packages/Microting.eForm/)


A .NET SDK for integrating with the Microting eForm API v1.

## Support Platforms

 - .NET Framework 4.5+
 - Windows 7 SP1
 - MS SQL 2008 R2

## Setup

To get started with Microting eForm SDK, we recommend you to install the SDK using NuGet with 

```
PM> Install-Package Microting.eForm
```

[We also recommend you to have a look at the reference Angular/C# web frontend](https://github.com/microting/eform-angular-frontend)

At this project you will be able to see best practice for using our SDK.

## Get access token

You need to create an account for Microting API and get your access credentials.

 - Call Microting at +45 66 11 10 66 to get started.

## Docs

[SDK documentation can be found here (beta)](https://microting.github.io/eform-sdk-documentation/?csharp#)

## Examples, demonstrating different use cases.

Several examples can be found in the Program.cs file:
* method Sample1 : Used for simple work orders, a work order is generated and an eForm is send to one device, when the device returns the result, the eForm is no longer visible on the device.
* method Sample2 : Used for letting multiple people receive the same work order, but only use the first returned. An eForm is send to several devices, when one device returns a result, the eForm is no longer visible on that device, and results from other devices are stored, but marked as retracted.
* method Sample3 : Used for ordering a service and let handlers bit on being the handler. An ordering eForm is send to several devices. One devices returns a result with an order, this sends out eForm's to handlers, the first handler to accept, will receive the order. The winning handler gets the next eForm. The loosing handlers gets information about they didn't win. And those who didn't bit will have their eForm's retracted.

## Development recommendations

  - Visual Studio 2008
  - MS SQL Server Management Studio
  
## Known bugs as of February 21st 2017

  - The Core.TemplatFromXml(string xmlString) is not able to parse DataItems of the following types:
	- EntitySearch
	- EntitySelect

## Changelog

[Changelog](changelog.md)

## Upgrading Database

You need to run Update-Database from within NuGet Package Manager with eFormSqlController set as *Default project*.	
	
## License

The MIT License (MIT)

Copyright (c) 2007-2018 microting

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
