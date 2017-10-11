## Changelog

  - Version 1.6.3.1 *(October 10th 2017)*
	- Minor update, moving some dedications.

  - Version 1.6.3.0 *(October 9th 2017)*
	- Extracting the jasper xml generator method, so it can be called directly. 
	- Adding logging if the case can not be deleted because of an error on server side. 
	- Fixed Core.Restart and tests added.

  - Version 1.6.2.8 *(September 19th 2017)*
	- Fixing the broken code inside TemplateItemReadAll and CaseDelete.

  - Version 1.6.2.6 *(September 19th 2017)*
	- Fixing the workflow_state setting for loading deployed sites.

  - Version 1.6.2.5 *(September 18th 2017)*
	- Improving the consistency check code.

  - Version 1.6.2.4 *(September 18th 2017)*
	- Fixing the field list to be a FieldDto instead of a fields object.
	- Adding method to try to consistency check db, first step.
	- Not_found event added

  - Version 1.6.2.3 *(September 15th 2017)*
	- Improved settings loading and defaulting. 
	- Initial support for MySQL - Beta.
	- Fixing the missing CDATA for name and description for EntitySearch and EntitySearchGroup.
	- Including the fields when doing a TemplateItemRad.

  - Version 1.6.2.2 *(September 11th 2017)*
	- Adding the missing Advanced_TemplateFieldReadAll on the core.

  - Version 1.6.2.1 *(September 7th 2017)*
	- Improved handling of CaseDelete errors from server.

  - Version 1.6.2.0 *(September 6th 2017)*
	- Better self healing for broken settings.

  - Version 1.6.1.9 *(September 5th 2017)*
	- Adding the changes to make the AdminTool.DbSetup(token) work.

  - Version 1.6.1.8 *(September 4th 2017)*
	- Adding the missing id for the uploadedDataObj. 
	- Added better logging for the moving file.

  - Version 1.6.1.7 *(September 1st 2017)*
	- Making the settingscheck be more explicit about which setting is missing if any.

  - Version 1.6.1.6 *(August 31st 2017)*
	- Fixing the startSqlOnly to set core as running. 
	- Changing max lenght of default value for a field.

  - Version 1.6.1.5 *(August 31st 2017)*
	- Enabling so the AdminTool constructor can be initialized, so subsequent methods can be called.

  - Version 1.6.1.4 *(August 31st 2017)*
	- Adding Advanced_DeleteUploadedData. Adding first steps for MySQL support.

  - Version 1.6.1.3 *(August 15th 2017)*
	- Fixing the broken listing of all cases in ReadAllCase.

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