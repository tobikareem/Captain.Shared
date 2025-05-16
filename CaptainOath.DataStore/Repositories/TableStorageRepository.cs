using Azure.Data.Tables;
using CaptainOath.DataStore.Interface;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace CaptainOath.DataStore.Repositories;

public class TableStorageRepository<T> : ITableStorageRepository<T> where T : class, ITableEntity, new()
{
    private readonly TableServiceClient _tableServiceClient;
    private readonly ILogger<TableStorageRepository<T>> _logger;

    public TableStorageRepository(ILogger<TableStorageRepository<T>> logger, TableServiceClient tableServiceClient)
    {
        _tableServiceClient = tableServiceClient;
        _logger = logger;
    }


    public async Task CreateTableAsync(string tableName)
    {
        await _tableServiceClient.CreateTableIfNotExistsAsync(tableName);
    }

    public async Task InsertTableEntityAsync(string tableName, T entity)
    {
        var client = _tableServiceClient.GetTableClient(tableName);
        var response = await client.AddEntityAsync(entity);

        _logger.LogInformation($"Inserted entity with PartitionKey: {entity.PartitionKey} and RowKey: {entity.RowKey}. Status: {response.Status}");
    }

    public async Task UpsertTableEntityAsync(string tableName, T entity)
    {
        var client = _tableServiceClient.GetTableClient(tableName);
        await client.UpsertEntityAsync(entity);
        _logger.LogInformation($"Upserted entity with PartitionKey: {entity.PartitionKey}, RowKey: {entity.RowKey}");
    }

    public async Task<T> GetTableEntityAsync(string tableName, string partitionKey, string rowKey)
    {
        var client = _tableServiceClient.GetTableClient(tableName);
        return await client.GetEntityAsync<T>(partitionKey, rowKey);

    }

    public async Task UpdateTableEntityAsync(string tableName, T entity)
    {
        var client = _tableServiceClient.GetTableClient(tableName);
        await client.UpdateEntityAsync(entity, entity.ETag, TableUpdateMode.Replace);
        _logger.LogInformation($"Updated entity with PartitionKey: {entity.PartitionKey}, RowKey: {entity.RowKey}");

    }

    public async Task DeleteTableEntityAsync(string tableName, string partitionKey, string rowKey)
    {
        var client = _tableServiceClient.GetTableClient(tableName);
        await client.DeleteEntityAsync(partitionKey, rowKey);
    }

    public async Task DeleteTableAsync(string tableName)
    {
        await _tableServiceClient.DeleteTableAsync(tableName);
        _logger.LogInformation($"Deleted table: {tableName}");
    }

    public async Task<IEnumerable<T>> GetTableEntitiesByQueryAsync(string tableName, string query)
    {
        var client = _tableServiceClient.GetTableClient(tableName);
        var queryResult = client.QueryAsync<T>(filter: query);
        var results = await queryResult.AsPages().ToListAsync();
        return results.SelectMany(page => page.Values);
    }

    public async Task<(List<T> Items, string ContinuationToken)> GetTableEntitiesPagedAsync(string tableName, string query, int maxResults, string continuationToken = null)
    {
        var client = _tableServiceClient.GetTableClient(tableName);

        var queryFilter = string.IsNullOrWhiteSpace(query)
            ? client.QueryAsync<T>(maxPerPage: maxResults)
            : client.QueryAsync<T>(filter: query, maxPerPage: maxResults);

        var page = await queryFilter.AsPages(continuationToken, maxResults).FirstOrDefaultAsync();

        return page == null ? (new List<T>(), null) : (page.Values.ToList(), page.ContinuationToken);
    }
}