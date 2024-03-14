using Azure.Data.Tables;
using Azure.Identity;
using CaptainOath.DataStore.Interface;
using CaptainOath.DataStore.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace CaptainOath.DataStore.Extension;

public static class TableStorageExtension
{
    /// <summary>
    /// Register the Table client with the service collection and provide the configurations.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="accountEndPointUrl"></param>
    /// <returns></returns>
    public static IServiceCollection AddTableStorageClient(this IServiceCollection services, string accountEndPointUrl)
    {

        services.AddSingleton<ITableStorageRepository<TableEntity>, TableStorageRepository<TableEntity>>();

        services.AddSingleton(_ =>
        {

            var credentials = new DefaultAzureCredential();

            var tableServiceClient = new TableServiceClient(new Uri(accountEndPointUrl), credentials);

            return tableServiceClient;


        });

        return services;
    }
}