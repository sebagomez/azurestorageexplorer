[![Build status](https://ci.appveyor.com/api/projects/status/d102wxr3nv6dqvi2?svg=true)](https://ci.appveyor.com/project/sebagomez/azurestorageexplorer)
[![Stories in Ready](https://badge.waffle.io/sebagomez/azurestorageexplorer.png?label=ready&title=Ready)](https://waffle.io/sebagomez/azurestorageexplorer)
[![Join the chat at https://gitter.im/sebagomez/azurestorageexplorer](https://badges.gitter.im/sebagomez/azurestorageexplorer.svg)](https://gitter.im/sebagomez/azurestorageexplorer?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

Try it live at https://azurestorage.azurewebsites.net

Or deploy it to your own Azure WebSite  
[![Deploy to Azure](http://azuredeploy.net/deploybutton.png)](https://azuredeploy.net/?repository=https://github.com/sebagomez/azurestorageexplorer) <-- [coolest Azure feature](http://sgomez.blogspot.com.uy/2014/11/deploy-to-azure-button-wow.html) ever!

Azure Storage Explorer
======================
Windows Azure Web Storage Explorer makes it easier for developers to browse and manage Blobs, Queues and Tables from Windows Azure Storage account. You'll no longer have to install a local client to do that. It's developed in C#.

![Screenshot](GitMain.png)


**Blobs**: Create public or private Containers and Blobs (only BlockBlobs for now). Download or delete your blobs.

**Queues**: Create Queues and messages.

**Tables**: Create table and Entities. To create an Entity you'll have to add one property per line in the form of `<PropertyName>:<PropertyValue>`

If you don't set PertitionKey or RowKey default values will be used ("1" for PartitionKey and a new Guid for RowKey).
For Example to create a new movie:
> PartitionKey:Action
RowKey:1
Title:Die Hard

To query the entities from a table use the following syntax: `<PropertyName> [operator] <ProepertyValue>`
Where the valid operators are:  *eq* (equals), *gt* (greater than), *ge* (greater or equal), *lt* (less than), *le* (less or equal) and *ne* (not equal).   
To query action movies use the following:
> PartitionKey eq 'Action'  

*Please note there's a <kbd>space</kbd> character before and after the eq operator.*

![Screenshot](Tables.png)

If you don't write a query the system will retrieve every Entity on the Table
