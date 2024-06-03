using Microsoft.AspNetCore.Http;

namespace CaptainOath.DataStore.Interface;

/// <summary>
/// Interface for interacting with Azure Blob Storage using BlobClient.
/// </summary>
public interface IBlobClientRepository
{
    /// <summary>
    /// Checks if a blob exists.
    /// </summary>
    /// <returns>True if the blob exists, otherwise false.</returns>
    Task<bool> DoesBlobExistAsync();

    /// <summary>
    /// Downloads a blob as a string.
    /// </summary>
    /// <returns>The blob data as a string.</returns>
    Task<string> DownloadBlobAsStringAsync();

    /// <summary>
    /// Downloads a blob as a stream.
    /// </summary>
    /// <returns>The blob data as a stream.</returns>
    Task<Stream> DownloadBlobAsStreamAsync();

    /// <summary>
    /// Uploads a blob with specified content.
    /// </summary>
    /// <param name="content">The content of the blob in string format.</param>
    /// <param name="contentType">The content type of the blob.</param>
    /// <param name="metadata">Metadata for the blob.</param>
    /// <returns>The URI of the uploaded blob.</returns>
    Task<string> UploadBlobAsync(string content, string contentType = "text/plain", IDictionary<string, string> metadata = null);

    /// <summary>
    /// Uploads a file as an attachment.
    /// </summary>
    /// <param name="file">The file to upload.</param>
    /// <returns>The URI of the uploaded file.</returns>
    Task<string> UploadAttachmentAsync(IFormFile file);

    /// <summary>
    /// Uploads multiple files as attachments.
    /// </summary>
    /// <param name="files">The files to upload.</param>
    /// <returns>A list of URIs for the uploaded files.</returns>
    Task<List<string>> UploadAttachmentsAsync(IEnumerable<IFormFile> files);

    /// <summary>
    /// Downloads a blob using its absolute URI.
    /// </summary>
    /// <param name="absoluteUri">The absolute URI of the blob.</param>
    /// <returns>The blob data as a string.</returns>
    Task<string> DownloadAttachmentUsingUriAsync(string absoluteUri);
}