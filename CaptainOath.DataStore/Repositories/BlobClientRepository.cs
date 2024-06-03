using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text;
using CaptainOath.DataStore.Interface;

namespace CaptainOath.DataStore.Repositories;

/// <summary>
/// Implementation of IBlobClientRepository for interacting with Azure Blob Storage using BlobClient.
/// </summary>
public class BlobClientRepository(ILogger<BlobClientRepository> logger, BlobClient blobClient)
    : IBlobClientRepository
{
    /// <summary>
    /// Checks if a blob exists.
    /// </summary>
    /// <returns>True if the blob exists, otherwise false.</returns>
    public async Task<bool> DoesBlobExistAsync()
    {
        return await blobClient.ExistsAsync();
    }

    /// <summary>
    /// Downloads a blob as a stream.
    /// </summary>
    /// <returns>The blob data as a stream.</returns>
    public async Task<Stream> DownloadBlobAsStreamAsync()
    {
        var download = await blobClient.DownloadStreamingAsync();
        return download.Value.Content;
    }

    /// <summary>
    /// Downloads a blob as a string.
    /// </summary>
    /// <returns>The blob data as a string.</returns>
    public async Task<string> DownloadBlobAsStringAsync()
    {
        BlobDownloadResult downloadResult = await blobClient.DownloadContentAsync();
        var downloadedData = downloadResult.Content.ToString();
        return downloadedData;
    }

    /// <summary>
    /// Uploads a blob with specified content.
    /// </summary>
    /// <param name="content">The content of the blob in string format.</param>
    /// <param name="contentType">The content type of the blob.</param>
    /// <param name="metadata">Metadata for the blob.</param>
    /// <returns>The URI of the uploaded blob.</returns>
    public async Task<string> UploadBlobAsync(string content, string contentType = "text/plain", IDictionary<string, string> metadata = null)
    {
        using var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(content));

        var blobHttpHeaders = new BlobHttpHeaders { ContentType = contentType };

        await blobClient.UploadAsync(memoryStream, new BlobUploadOptions
        {
            HttpHeaders = blobHttpHeaders,
            Metadata = metadata
        });

        logger.LogInformation($"Uploaded blob to Blob storage.");
        return blobClient.Uri.AbsoluteUri;
    }

    /// <summary>
    /// Uploads a file as an attachment.
    /// </summary>
    /// <param name="file">The file to upload.</param>
    /// <returns>The URI of the uploaded file.</returns>
    public async Task<string> UploadAttachmentAsync(IFormFile file)
    {
        await using (var stream = file.OpenReadStream())
        {
            await blobClient.UploadAsync(stream, overwrite: true);
        }

        logger.LogInformation($"File {file.FileName} uploaded to Blob storage.");

        return blobClient.Uri.AbsoluteUri;
    }

    /// <summary>
    /// Uploads multiple files as attachments.
    /// </summary>
    /// <param name="files">The files to upload.</param>
    /// <returns>A list of URIs for the uploaded files.</returns>
    public async Task<List<string>> UploadAttachmentsAsync(IEnumerable<IFormFile> files)
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
                await using (var stream = file.OpenReadStream())
                {
                    await blobClient.UploadAsync(stream, overwrite: true);
                }

                logger.LogInformation($"File {file.FileName} uploaded to Blob storage.");

                uris.Add(blobClient.Uri.AbsoluteUri);
            }
            catch (Exception ex)
            {
                logger.LogError($"An error occurred while uploading file {file.FileName}. Error: {ex.Message}");
            }
        }

        return uris;
    }

    /// <summary>
    /// Downloads a blob using its absolute URI.
    /// </summary>
    /// <param name="absoluteUri">The absolute URI of the blob.</param>
    /// <returns>The blob data as a string.</returns>
    public Task<string> DownloadAttachmentUsingUriAsync(string absoluteUri)
    {
        throw new NotImplementedException();
    }
}