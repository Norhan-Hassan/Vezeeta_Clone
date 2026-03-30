namespace Vezeeta_Clone.Service.ExternalServices.Abstract
{
    public interface IBlobStorageService
    {
        string GetBlobUrl(string containerName, string blobName);
        string GetBlobUrlmedia(string MediaContent);
        Task DownloadBlobAsync(string blobName, string downloadFilePath);
        Task<string> UploadBlobAsync(string containerName, string blobName, Stream content, string contentType);
    }
}
