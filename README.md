# eform-sdk-dotnet 

![C# Build status on Windows](https://microtingas2017.visualstudio.com/_apis/public/build/definitions/5f551ab2-01ab-4204-8efa-06be93328bc1/1/badge)

[![Build Status on Linux](https://travis-ci.org/microting/eform-sdk-dotnet.svg?branch=master)](https://travis-ci.org/microting/eform-sdk-dotnet)

[![NuGet Badge](https://buildstats.info/nuget/Microting.eForm)](https://www.nuget.org/packages/Microting.eForm/)


A .NET SDK for integrating with the Microting eForm API v1.

## Supported Platforms

| OS        | .Net Standard 2.0 | NET Framework 4.6.1 | MS SQL 2016+ | MySQL/MariaDB  |
| ------------- |:-----:|:-----:|:-----:|:-----:|
| OSX 10.14.2     | X |  | X | X |
| Windows 10     | X | X | X | X |
| Windows Server 2016     | X| X | X | X |
| Ubuntu 18.04     | X |  | X | X |
| Ubuntu 16.04     | X |  | X | X |

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

  - Visual Studio 2017 / Rider
  - MS SQL Server Management Studio / MySQL 5.5+
  
## Known bugs as of February 21st 2017

  - The Core.TemplatFromXml(string xmlString) is not able to parse DataItems of the following types:
	- EntitySearch
	- EntitySelect

## Changelog

[Changelog](changelog.md)


## Contributing

1. Do a fork
2. Clone your fork onto your own computer
3. Checkout/create a new branch for your relevant issue
4. Apply your changes and tests
5. Commit your changes and push to github
6. Create a pull-request

### Pull requests

To enable us to quickly review and accept your pull requests, always create one pull request per issue and link the issue in the pull request. Never merge multiple requests in one unless they have the same root cause. Be sure to follow our coding guidelines and keep code changes as small as possible. Avoid pure formatting changes to code that has not been modified otherwise. Pull requests should contain tests whenever possible.

Pull-reuqsts that do not pass tests, will not be accepted.

### Where to contribute

Check out the [full issues list](https://github.com/microting/eform-sdk-dotnet/issues) for a list of all potential areas for contributions.

To improve the chances to get a pull request merged you should select an issue that is labelled with the [help_wanted](https://github.com/microting/eform-sdk-dotnet/issues?q=is%3Aissue+is%3Aopen+label%3Ahelp_wanted) or [bug](https://github.com/microting/eform-sdk-dotnet/issues?q=is%3Aissue+is%3Aopen+label%3Abug) labels. If the issue you want to work on is not labelled with `help-wanted` or `bug`, you can start a conversation with the issue owner asking whether an external contribution will be considered.
	
### Suggestions

We're also interested in your feedback for the future of Microting eForm SDK. You can submit a suggestion or feature request through the issue tracker. To make this process more effective, we're asking that these include more information to help define them more clearly.

## Adding new migrations

Where MIGRATIONNAME is the name of the migration you want to create
```powershell
dotnet ef migrations add MIGRATIONNAME --project eFormCore/Microting.eForm.csproj --startup-project SourceCode/SourceCode.csproj --context MicrotingDbAnySQL
```

## Microting Open Source Code of Conduct

This project has adopted the [Microting Open Source Code of Conduct](https://www.microting.com/microting-open-source-code-of-conduct/). Contact opencode@microting.com with any additional questions or comments.
	
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
