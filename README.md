Try it live at http://azurestorage.azurewebsites.net

Or deploy it to your own Azure WebSite  
[![Deploy to Azure](http://azuredeploy.net/deploybutton.png)](https://azuredeploy.net/?repository=https://github.com/sebagomez/azurestorageexplorer) <-- coolest Azure feature ever!

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

![Screenshot](Tables.png)

If you don't write a query the system will retrieve every Entity on the Table
