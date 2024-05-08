using Azure.Data.Tables;
using Azure.Identity;
using CaptainOath.DataStore.Interface;
using CaptainOath.DataStore.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace CaptainOath.DataStore.Extension;

public static class BlobStorageExtension
{
    public static IServiceCollection AddBlobStorageClient(this IServiceCollection services, string accountEndPointUrl)
    {
        services.AddScoped<ITableStorageRepository<TableEntity>, TableStorageRepository<TableEntity>>();


        services.AddSingleton(_ =>
        {

            var credentials = new DefaultAzureCredential();

            var tableServiceClient = new TableServiceClient(new Uri(accountEndPointUrl), credentials);

            return tableServiceClient;


        });

        return services;
    }
}