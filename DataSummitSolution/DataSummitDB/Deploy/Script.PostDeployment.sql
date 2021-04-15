/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
To use this script, right mouste button > Execution Settings > SQLCMD Mode

Note: This is a seeding only data set. Only to run on empty databases
--------------------------------------------------------------------------------------
*/

:r ..\dbo\Data\AspNetRoles.sql
:r ..\dbo\Data\AspNetUsers.sql
:r ..\dbo\Data\DocumentTypes.sql
:r ..\dbo\Data\BlockPositions.sql
:r ..\dbo\Data\PaperSizes.sql
:r ..\dbo\Data\StandardAttributes.sql
:r ..\dbo\Data\AzureCompanyResourceUrls.sql
:r ..\dbo\Data\AzureMLResources.sql
:r ..\dbo\Data\Projects.sql
:r ..\dbo\Data\PaperOrientations.sql
:r ..\dbo\Data\TemplateVersions.sql
:r ..\dbo\Data\Documents.sql
:r ..\dbo\Data\ImageGrids.sql
:r ..\dbo\Data\Sentences.sql
:r ..\dbo\Data\FunctionTasks.sql
:r ..\dbo\Data\GoogleLanguages.sql
:r ..\dbo\Data\TemplateAttributes.sql
:r ..\dbo\Data\Properties.sql