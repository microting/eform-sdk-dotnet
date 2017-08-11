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

## Docs

[SDK documentation can be found here (beta)](https://microting.github.io/eform-sdk-dotnet/)

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

  - Version 1.6.1.2 *(August 11th 2017)*
	- Adding the posibility to search in the cases based on field_values.
	- **Breaking changes to the method CaseReadAll argument list.**
	- To have the same behaviour as before, set searchKey to null.
	- Before
	```cs
	List<Case>  CaseReadAll(int? templatId, DateTime? start, DateTime? end, string workflowState)
	```
	- After
	```cs
	List<Case>  CaseReadAll(int? templatId, DateTime? start, DateTime? end, string workflowState, string searchKey)
	```	
	
  - Version 1.6.1.1 *(August 10th 2017)*
	- Disabling automatic migration and only using explicit migrations.

  - Version 1.6.1.0 *(August 10th 2017)*
	- Added the functionality to allow the user to set which field values should be represented on a given case for optimized viewing.

  - Version 1.6.0.3 *(August 9th 2017)*
	- Fixing the broken saving of latitude for the GPS coordinates.

  - Version 1.6.0.2 *(August 3rd 2017)*
	- Fixing the log of null value and better upgrading of settings table.

  - Version 1.6.0.0 *(August 2nd 2017)*
	- Improving the logging.
	
  - Version 1.5.5.4 *(June 27th 2017)*
	- Adding ability to change name of the EntityGroup.
	- **Breaking changes to the result EntityGroupCreate is returning.**
	- Before
	```cs
	string EntityGroupCreate(string entityType, string name);
	```
	- After
	```cs
	EntityGroup EntityGroupCreate(string entityType, string name);
	```	

  - Version 1.5.5.3 *(June 20th 2017)*
	- Fixing the broken set display index for an already deployed eForm.

  - Version 1.5.5.2 *(June 20th 2017)*
	- Fixing the broken set display index for an already deployed eForm.

  - Version 1.5.5.1 *(June 19th 2017)*
	- Adding the workflowState parameter to the Advanced_EntityGroupAll.

  - Version 1.5.5.0 *(June 19th 2017)*
	- Adding created_at and updated_at to EntityGroup.

  - Version 1.5.4.8 *(June 19th 2017)*
	- Fixing the missing initalization of List<EntityGroup> in EntityGroupAll of the SqlController.

  - Version 1.5.4.7 *(June 16th 2017)*
	- Adding display index to Template_Dto.

  - Version 1.5.4.6 *(June 16th 2017)*
	- Adding entityType and desc to Advanced_EntityGroupAll.

  - Version 1.5.4.5 *(June 14th 2017)*
	- Adding Advanced_EntityGroupAll to list all EntityGroups.

  - Version 1.5.4.4 *(June 14th 2017)*
	- Fixing recreating removed EntityItems.

  - Version 1.5.4.3 *(June 13th 2017)*
	- Fixing the broken URL in the change display index call.

  - Version 1.5.4.2 *(June 13th 2017)*
	- Adding functionality to allow the user to update the display index of eForms already deployed.

  - Version 1.5.4.1 *(June 1st 2017)*
	- Fixing CaseCreate to check for existing case before creating a new one.
	- Advanced_InteractionCaseDelete(int interactionCaseId) added
	- CaseDelete improved
	- Fixed multiple interaction cases create	

  - Version 1.5.4 *(May 9th 2017)*
	- **Breaking changes to the way CaseCreate is used.**
	- Before
	```cs
	CaseCreate(MainElement mainElement, string caseUId, List<int> siteIds, string custom, bool reversed);
	```
	- After
	```cs
	CaseCreate(MainElement mainElement, string caseUId, List<int> siteIds, string custom);
	```
	- If you previously had reversed set to true, then you have to set mainElement.Repeated = 0 or any number higher than 1
	- If you previously had reversed set to false, then you have to set mainElement.Repeated = 1

  - Version 1.5.3.1 *(May 8th 2017)*
	- Fixing sync of EntityItems.

  - Version 1.5.3 *(May 2nd 2017)*
	- Making the Advanced_WorkerReadAll have parameters for workflowState, offSet and limit.
		- Advanced_WorkerReadAll now takes (string workflowState, int? offSet, int? limit)
	- Removing the not implemented siteworkerupdate method.
	- Adding the missing delete calls to api endpoint.
	- Fixing == which should be != in SqlController.

  - Version 1.5.2.9 *(April 29th 2017)*
	- Adding MigrateDb to AdminTools, so the implementer is able to force migration of the DB.
		- **It is recommended to run AdminTools.MigrateDb every time there has been an upgrade of the NuGet!**.

  - Version 1.5.2.8 *(April 27th 2017)*
	- Adding Created at to the columns in the CSV export.
	- Extending the description columns for check_lists and fields.

  - Version 1.5.2.7 *(April 25th 2017)*
	- Fixing the timestamp for cases.created_at
	- Fixing the selection of field_values for the CSV generator

  - Version 1.5.2.5 *(April 25th 2017)*
	- Fixing the broken CSV generator for removed cases.

  - Version 1.5.2.4 *(April 20th 2017)*
	- CVS "" added, core close sorted
	- Fixing the handling of threads stopping, to make the core able to restart gracefully
	- Fixing CaseCreate broken check

  - Version 1.5.2.3 *(April 20th 2017)*
	- Fixing the broken migration for notifications

  - Version 1.5.2.2 *(April 18th 2017)*
	- Fixing the broken migration
	- If you get an error including "Invalid coulmn name 'template_id'" "Invalid column name 'replacements'" then you should drop the table: a_input_cases and run the below script:
	```sql
	CREATE TABLE [dbo].[a_input_cases] (
		[id] [int] NOT NULL IDENTITY,
		[workflow_state] [varchar](255),
		[created_at] [datetime2](0),
		[updated_at] [datetime2](0),
		[site_uids] [varchar](max),
		[case_uid] [varchar](255),
		[custom] [varchar](max),
		[reversed] [smallint],
		[microting_uids] [varchar](255),
		[connected] [smallint],
		[template_id] [int],
		[replacements] [varchar](max),
		CONSTRAINT [PK_dbo.a_input_cases] PRIMARY KEY ([id])
	)
	CREATE TABLE [dbo].[a_output_cases] (
		[id] [int] NOT NULL IDENTITY,
		[workflow_state] [varchar](255),
		[created_at] [datetime2](0),
		[updated_at] [datetime2](0),
		[microting_uid] [varchar](255),
		[check_uid] [varchar](max),
		[check_list_id] [int],
		[stat] [varchar](255),
		[site_uid] [varchar](max),
		[case_type] [varchar](max),
		[case_uid] [varchar](255),
		[custom] [varchar](max),
		[case_id] [int],
		CONSTRAINT [PK_dbo.a_output_cases] PRIMARY KEY ([id])
	)
	```	

  - Version 1.5.2.1 *(April 7th 2017)*
	- Fixing the url for pdf upload
	- Adding migration to drop outlook table.
	- Fixing the core not started exception for startSql.

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
