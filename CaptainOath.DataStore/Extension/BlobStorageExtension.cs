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

    public static IServiceCollection AddBlobStorageContainerUserAssigned(this IServiceCollection services, string accountEndPointUrl, string containerName, string userAssignedManagedClientId)
    {
        services.AddScoped<IBlobStorageRepository, BlobContainerRepository>();

        var credentials = new DefaultAzureCredential();

        services.AddSingleton(_ =>
        {
            var blobServiceClient = new BlobServiceClient(new Uri(accountEndPointUrl), credentials);
            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            return containerClient;

        });

        return services;
    }

    public static IServiceCollection AddBlobStorageContainerUserAssignedManagedIdentity(this IServiceCollection services, string accountEndPointUrl, string containerName, string userAssignedManagedClientId)
    {
        services.AddScoped<IBlobStorageRepository, BlobContainerRepository>();

        var credentials = new DefaultAzureCredential(new DefaultAzureCredentialOptions { ManagedIdentityClientId = userAssignedManagedClientId });

        services.AddSingleton(_ =>
        {
            var blobServiceClient = new BlobServiceClient(new Uri(accountEndPointUrl), credentials);
            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            return containerClient;

        });

        return services;
    }

    public static IServiceCollection AddBlobStorageContainerLocal(this IServiceCollection services, string accountEndPointUrl, string containerName)
    {
        services.AddScoped<IBlobStorageRepository, BlobContainerRepository>();

        services.AddSingleton(_ =>
        {
            var credentials = new DefaultAzureCredential(new DefaultAzureCredentialOptions { ExcludeManagedIdentityCredential = true });
            var blobServiceClient = new BlobServiceClient(new Uri(accountEndPointUrl), credentials);
            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            return containerClient;
        });

        return services;
    }

    public static IServiceCollection AddBlobClientUri(this IServiceCollection services, string blobUri)
    {
        services.AddScoped<IBlobClientRepository, BlobClientRepository>();

        services.AddSingleton(_ =>
        {
            var blobClient = new BlobClient(new Uri(blobUri));
            return blobClient;
        });

        return services;
    }

    public static IServiceCollection AddBlobStorageClientUsingConnectionString(this IServiceCollection services, string connectionString)
    {
        services.AddScoped<IBlobStorageRepository, BlobStorageRepository>();

        services.AddSingleton(_ =>
        {
            var blobServiceClient = new BlobServiceClient(connectionString);
            return blobServiceClient;
        });

        return services;
    }


}