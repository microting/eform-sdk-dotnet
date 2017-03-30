# eform-sdk-dotnet

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

[We also recommend you to have a look at the reference web frontend](https://github.com/microting/eform-frontend-dotnet)

At this project you will be able to see best practice for using our SDK.

## Get access token

You need to create an account for Microting API and get your access credentials.

 - Call Microting at +45 66 11 10 66 to get started.

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

  - Version 1.5.2.0 *(March 30th 2017)*
	- Adding support for handling PDF fields..
	- Run AdminTools.DbSettingsReloadRemote prio using Core the first time. This will load the new settings from API endpoint.

  - Version 1.5.1.6 *(March 29th 2017)*
	- Fixing the return value in Core.CasesToCsv and Core.CaseRead for EntitySearch and EntitySelect.

  - Version 1.5.1.5 *(March 28th 2017)*
	- Fixing the situation, where no value is selected for an EntitySearch or EntitySelect field and not all items have been synced with the API.

  - Version 1.5.1.4 *(March 23th 2017)*
	- Making the prime DB not owerwrite settings if they are incorrect.

  - Version 1.5.1.3 *(March 23th 2017)*
	- Fixing correct customer no saved to units table.

  - Version 1.5.1.2 *(March 22th 2017)*
	- Fixing binding issue for deleting site internally.

  - Version 1.5.1.1 *(March 22th 2017)*
	- Fixing API endpoint timeout.

  - Version 1.5.1 *(March 21th 2017)*
	- Adding missing AWS dependencies.

  - Version 1.5.0 *(March 20th 2017)*
	- Making all methods for advanced purposes to be prefixed with advanced.

  - Version 1.4.1 *(March 8th 2017)*
	- Fixing the string length of description on check_lists and fields.

  - Version 1.4.0 *(March 7th 2017)*
	- **Breaking changes to db structure.**
		- Upgrading from 1.3.x to version 1.4.0 is not possible automatically.
		- The following tables data needs to be altered by hand or script:
			- cases
			- case_versions
			- field_values
			- field_value_versions
			- check_list_value
			- check_list_value_versions
			- site_workers
			- site_worker_versions
			- units
			- unit_versions
		- For some cases the following tables data also needs to be modified by hand or script.
			- check_list_sites
			- check_list_site_versions
	- Adding AdminTools to prime the DB and ease the setup of new installations.

  - Version 1.3.3 *(February 22st 2017)*
	- Fixing parsing of PDF field types.
	- Merging eFormRequest and eFormResponse into eFormData.
	- Added TemplatValidation helper on Core to check the input XML for unsupported values, including:
		- PDF
		- EntitySelect
		- EntitySearch
	- eForm with repetition other than 1 are marked as retracted when completed.

  - Version: 1.3.1 *(February 16th 2017)*
	- Fixing reverse cases.
	- Adding changes to make SDK migrate DB automatically.
	- Adding changes to not crash on missing first_run.txt file.

  - Version: 1.3.0 *(February 8th 2017)*
	- **Breaking changes to db structure.**
	- Introducing usage of EntityFramework for migrating db.
	- Making db use keys + foeing keys, which enables use of EntityFramework.
	- Loading of existing sites/units/workers from Microting API.

## Upgrading Database

You need to run Update-Database from within NuGet Package Manager with eFormSqlController set as *Default project*.	
	
## License

The MIT License (MIT)

Copyright (c) 2007-2017 microting

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
