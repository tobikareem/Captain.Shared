using Microsoft.AspNetCore.Http;
namespace CaptainOath.DataStore.Interface;

public interface IBlobStorageRepository
{
    /// <summary>
    /// Checks if a blob exists in a specified container.
    /// </summary>
    /// <param name="fileName">The name of the blob file.</param>
    /// <param name="blobContainerName">The name of the blob container.</param>
    /// <returns>True if the blob exists, otherwise false.</returns>
    /// <exception cref="Exception">Throws if there's an error accessing the storage.</exception>
    Task<bool> DoesBlobExistAsync(string fileName, string blobContainerName);

    /// <summary>
    /// Downloads a blob from a specified container.
    /// </summary>
    /// <param name="fileName">The name of the blob file.</param>
    /// <param name="blobContainerName">The name of the blob container.</param>
    /// <returns>The blob data as a string.</returns>
    /// <exception cref="Exception">Throws if there's an error during the download.</exception>
    Task<string> DownloadBlobAsStringAsync(string fileName, string blobContainerName);

    /// <summary>
    /// Downloads a blob from a specified container.
    /// </summary>
    /// <param name="fileName">The name of the blob file.</param>
    /// <param name="blobContainerName">The name of the blob container.</param>
    /// <returns>The blob data as a string.</returns>
    /// <exception cref="Exception">Throws if there's an error during the download.</exception>
    Task<Stream> DownloadBlobAsStreamAsync(string fileName, string blobContainerName);

    /// <summary>
    /// Uploads a blob to a specified container.
    /// </summary>
    /// <param name="fileName">The name of the blob file.</param>
    /// <param name="content">The content of the blob in string format.</param>
    /// <param name="blobContainerName">The name of the blob container.</param>
    /// <param name="contentType">Pass an appropriate content type</param>
    /// <param name="metadata">Pass metadata of the file</param>
    /// <returns>The URI of the uploaded blob.</returns>
    /// <exception cref="Exception">Throws if there's an error during the upload.</exception>
    Task<string> UploadBlobAsync(string fileName, string content, string blobContainerName, string contentType = "text/plain", IDictionary<string, string> metadata = null);

    /// <summary>
    /// Uploads a file as an attachment to a specified blob container.
    /// </summary>
    /// <param name="file">The file to upload.</param>
    /// <param name="blobContainerName">The name of the blob container.</param>
    /// <returns>The URI of the uploaded file.</returns>
    /// <exception cref="Exception">Throws if there's an error during the file upload.</exception>
    Task<string> UploadAttachmentAsync(IFormFile file, string blobContainerName);

    /// <summary>
    /// Uploads multiple files as attachments to a specified blob container.
    /// </summary>
    /// <param name="files">The files to upload.</param>
    /// <param name="blobContainerName">The name of the blob container.</param>
    /// <returns>A list of URIs for the uploaded files.</returns>
    /// <exception cref="Exception">Throws if there's an error during the file uploads.</exception>
    Task<List<string>> UploadAttachmentsAsync(IEnumerable<IFormFile> files, string blobContainerName);

    /// <summary>
    /// Downloads a blob using its absolute URI.
    /// </summary>
    /// <param name="absoluteUri">The absolute URI of the blob.</param>
    /// <returns>The blob data as a string.</returns>
    /// <exception cref="Exception">Throws if there's an error during the download.</exception>
    Task<string> DownloadAttachmentUsingUriAsync(string absoluteUri);
}
