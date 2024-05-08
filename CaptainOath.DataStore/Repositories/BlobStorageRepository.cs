using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using CaptainOath.DataStore.Interface;
using Microsoft.Extensions.Logging;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace CaptainOath.DataStore.Repositories;

public class BlobStorageRepository : IBlobStorageRepository
{
    private readonly ILogger<BlobStorageRepository> _logger;
    private readonly BlobServiceClient _blobServiceClient;

    public BlobStorageRepository(ILogger<BlobStorageRepository> logger, BlobServiceClient blobServiceClient)
    {
        _logger = logger;
        _blobServiceClient = blobServiceClient;
    }

    public async Task<bool> DoesBlobExistAsync(string fileName, string blobContainerName)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(blobContainerName);
        var blobClient = containerClient.GetBlobClient(fileName);

        return await blobClient.ExistsAsync();
    }

    public async Task<Stream> DownloadBlobAsStreamAsync(string fileName, string blobContainerName)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(blobContainerName);
        var blobClient = containerClient.GetBlobClient(fileName);

        var download = await blobClient.DownloadStreamingAsync();
        return download.Value.Content;
    }

    public async Task<string> DownloadBlobAsStringAsync(string fileName, string blobContainerName)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(blobContainerName);
        var blobClient = containerClient.GetBlobClient(fileName);

        BlobDownloadResult downloadResult = await blobClient.DownloadContentAsync();
        var downloadedData = downloadResult.Content.ToString();
        return downloadedData;
    }

    public async Task<string> UploadBlobAsync(string fileName, string content, string blobContainerName, string contentType = "text/plain", IDictionary<string, string> metadata = null)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(blobContainerName);
        var blobClient = containerClient.GetBlobClient(fileName);
        
        await containerClient.CreateIfNotExistsAsync();
        
        using var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(content));
        
        var blobHttpHeaders = new BlobHttpHeaders { ContentType = contentType };
        
        await blobClient.UploadAsync(memoryStream, new BlobUploadOptions
        {
            HttpHeaders = blobHttpHeaders,
            Metadata = metadata
        });
        
        _logger.LogInformation($"Uploaded blob '{fileName}' to container '{blobContainerName}'.");
        
        return blobClient.Uri.AbsoluteUri;
    }

    public async Task<string> UploadAttachmentAsync(IFormFile file, string blobContainerName)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(blobContainerName);

        await containerClient.CreateIfNotExistsAsync();

        var blobClient = containerClient.GetBlobClient(file.FileName);

        await using (var stream = file.OpenReadStream())
        {
            await blobClient.UploadAsync(stream, overwrite: true);
        }

        _logger.LogInformation($"File {file.FileName} uploaded to Blob storage.");

        return blobClient.Uri.AbsoluteUri;
    }

    public async Task<List<string>> UploadAttachmentsAsync(IEnumerable<IFormFile> files, string blobContainerName)
    {
        var uris = new List<string>();

        if (files == null)
        {
            return uris;
        }

        foreach (var file in files)
        {
            try
            {
                // Ensure you have a container name specified in your configuration
                var containerClient = _blobServiceClient.GetBlobContainerClient(blobContainerName);

                // If the container does not exist, create it
                // Note: It's not efficient to check this every time you upload a blob, consider handling container creation separately
                await containerClient.CreateIfNotExistsAsync();

                var blobClient = containerClient.GetBlobClient(file.FileName);

                await using (var stream = file.OpenReadStream())
                {
                    await blobClient.UploadAsync(stream, overwrite: true);
                }

                _logger.LogInformation($"File {file.FileName} uploaded to Blob storage.");

                uris.Add(blobClient.Uri.AbsoluteUri);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while uploading file {file.FileName}. Error: {ex.Message}");
            }
        }

        return uris;
    }

    public Task<string> DownloadAttachmentUsingUriAsync(string absoluteUri)
    {
        throw new NotImplementedException();
    }
}