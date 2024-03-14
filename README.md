# Captain.Shared Table Storage Repository

This nuget contains a .NET 8 class library repository application to connect to Azure Table Storage using managed identity.

## Getting Started

This is a nuget for data access. It provides a streamlined approach for integrating Azure Table Storage operations into .NET applications. It leverages Azure's managed identity for secure and simplified access, and offers a generic repository pattern for CRUD operations on Table Storage tables.

**Overview**

    - Create a table in the storage account
    - Insert an entity into the table
    - Insert or replace an entity into the table (Upsert)
    - Get an entity from the table by partition key and row key
    - Update an entity in the table
    - Delete an entity from the table by partition key and row key
    - Delete a table from the storage account
    - Get entities from the table by query

**Installation**
dotnet add package CaptainOath.DataStore


**Register Service**
    `services.AddTableStorageClient("accountEndPointUrl");`

**Using the Interface**
    `public class MyCosmosDbService
     {
        private readonly ITableStorageRepository<TableEntity> _repository;

        public MyTableStorageService(ITableStorageRepository<TableEntity> repository)
        {
            _repository = repository;
        }

        public async Task CreateMyEntityAsync(string tableName)
        {
             await _repository.CreateTableAsync(tableName);
        }

        public async Task InsertMyEntityAsync(string tableName, TableEntity entity)
        {
             await _repository.InsertTableEntityAsync(tableName, entity);
        }
     }
    `