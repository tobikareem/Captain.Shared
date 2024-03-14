namespace CaptainOath.DataStore.Interface;

/// <summary>
/// Interface for table storage
/// </summary>
/// <typeparam name="T"></typeparam>
public interface ITableStorageRepository <T> where T : class
{
    /// <summary>
    /// Create a table in the storage account
    /// </summary>
    /// <param name="tableName"></param>
    /// <returns></returns>
    Task CreateTableAsync(string tableName);

    /// <summary>
    /// Insert an entity into the table
    /// </summary>
    /// <param name="tableName"></param>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task InsertTableEntityAsync(string tableName, T entity);


    /// <summary>
    /// Insert or replace an entity into the table
    /// </summary>
    /// <param name="tableName"></param>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task UpsertTableEntityAsync(string tableName, T entity);


    /// <summary>
    /// Get an entity from the table by partition key and row key
    /// </summary>
    /// <param name="tableName"></param>
    /// <param name="partitionKey"></param>
    /// <param name="rowKey"></param>
    /// <returns></returns>
    Task<T> GetTableEntityAsync(string tableName, string partitionKey, string rowKey);

    /// <summary>
    /// Update an entity in the table
    /// </summary>
    /// <param name="tableName"></param>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task UpdateTableEntityAsync(string tableName, T entity);

    /// <summary>
    /// Delete an entity from the table by partition key and row key
    /// </summary>
    /// <param name="tableName"></param>
    /// <param name="partitionKey"></param>
    /// <param name="rowKey"></param>
    /// <returns></returns>
    Task DeleteTableEntityAsync(string tableName, string partitionKey, string rowKey);

    /// <summary>
    /// Delete a table from the storage account
    /// </summary>
    /// <param name="tableName"></param>
    /// <returns></returns>
    Task DeleteTableAsync(string tableName);

    /// <summary>
    /// Get entities from the table by query
    /// </summary>
    /// <param name="tableName"></param>
    /// <param name="query"></param>
    /// <returns></returns>
    Task<IEnumerable<T>> GetTableEntitiesByQueryAsync(string tableName, string query);

}