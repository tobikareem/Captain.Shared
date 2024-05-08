using Microsoft.AspNetCore.Http;

namespace CaptainOath.DataStore.Interface;

public interface IBlobStorageRepository
{
    Task<bool> DoesBlobExist(string fileName, string blobContainerName);
    Task<string> DownloadBlob(string fileName, string blobContainerName);
    Task UploadBlob(string fileName, string json, string blobContainerName);
    Task<string> UploadAttachmentAsync(IFormFile file, string blobContainerName);
    Task<List<string>> UploadAttachmentsAsync(IEnumerable<IFormFile> files, string blobContainerName);
    Task<string> DownloadAttachmentUsingUriAsync(string absoluteUri);
}