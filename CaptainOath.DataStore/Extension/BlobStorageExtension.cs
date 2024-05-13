using Azure.Identity;
using Azure.Storage.Blobs;
using CaptainOath.DataStore.Interface;
using CaptainOath.DataStore.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace CaptainOath.DataStore.Extension;

public static class BlobStorageExtension
{
    public static IServiceCollection AddBlobStorageClient(this IServiceCollection services, string accountEndPointUrl)
    {
        services.AddScoped<IBlobStorageRepository, BlobStorageRepository>();

        services.AddSingleton(_ =>
        {
            var credentials = new DefaultAzureCredential();
            var blobServiceClient = new BlobServiceClient(new Uri(accountEndPointUrl), credentials);
            return blobServiceClient;

        });

        return services;
    }

    public static IServiceCollection AddBlobStorageUserAssignedManagedIdentity(this IServiceCollection services, string accountEndPointUrl, string userAssignedManagedClientId)
    {
        services.AddScoped<IBlobStorageRepository, BlobStorageRepository>();

        services.AddSingleton(_ =>
        {
            var credentials = new DefaultAzureCredential(new DefaultAzureCredentialOptions { ManagedIdentityClientId = userAssignedManagedClientId });
            var blobServiceClient = new BlobServiceClient(new Uri(accountEndPointUrl), credentials);
            return blobServiceClient;

        });

        return services;
    }

}