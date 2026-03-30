using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Vezeeta_Clone.Data.Helper;
using Vezeeta_Clone.Service.ExternalServices.Abstract;

namespace Vezeeta_Clone.Service.ExternalServices.Implementations
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly AzureStorageSettings _azureStorageSettings;
        private readonly string _defaultContainerName;
        private readonly string _reportContainerName;

        public BlobStorageService(AzureStorageSettings azureStorageSettings)
        {
            _azureStorageSettings = azureStorageSettings;

            var connectionString = _azureStorageSettings.ConnectionString;

            if (string.IsNullOrEmpty(connectionString))
                throw new InvalidOperationException("Azure Storage connection string is not configured");

            _blobServiceClient = new BlobServiceClient(connectionString);
            _defaultContainerName = _azureStorageSettings.DefaultContainer;
            _reportContainerName = _azureStorageSettings.ReportContainer;
        }

        public string GetBlobUrl(string containerName, string blobName)
        {
            if (string.IsNullOrWhiteSpace(containerName))
                throw new ArgumentException("Container name cannot be empty", nameof(containerName));
            if (string.IsNullOrWhiteSpace(blobName))
                throw new ArgumentException("Blob name cannot be empty", nameof(blobName));

            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(blobName);
            return blobClient.Uri.ToString();
        }


        public string GetBlobUrlmedia(string mediaContent)
        {
            if (string.IsNullOrWhiteSpace(mediaContent))
                throw new ArgumentException("Media content name cannot be empty", nameof(mediaContent));

            return GetBlobUrl(_defaultContainerName, mediaContent);
        }

        public async Task DownloadBlobAsync(string blobName, string downloadFilePath)
        {
            if (string.IsNullOrWhiteSpace(blobName))
                throw new ArgumentException("Blob name cannot be empty", nameof(blobName));
            if (string.IsNullOrWhiteSpace(downloadFilePath))
                throw new ArgumentException("Download file path cannot be empty", nameof(downloadFilePath));

            var containerClient = _blobServiceClient.GetBlobContainerClient(_defaultContainerName);
            var blobClient = containerClient.GetBlobClient(blobName);

            var directory = Path.GetDirectoryName(downloadFilePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            try
            {
                await blobClient.DownloadToAsync(downloadFilePath);
            }
            catch (Azure.RequestFailedException ex)
            {
                throw new InvalidOperationException($"Failed to download blob '{blobName}': {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<string> UploadBlobAsync(string containerName, string blobName, Stream content, string contentType)
        {
            if (string.IsNullOrWhiteSpace(containerName))
                throw new ArgumentException("Container name cannot be empty", nameof(containerName));
            if (string.IsNullOrWhiteSpace(blobName))
                throw new ArgumentException("Blob name cannot be empty", nameof(blobName));
            if (content == null)
                throw new ArgumentNullException(nameof(content));

            try
            {
                var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
                var blobClient = containerClient.GetBlobClient(blobName);

                if (content.CanSeek)
                    content.Seek(0, SeekOrigin.Begin);

                await blobClient.UploadAsync(content, overwrite: true);

                var httpHeaders = new BlobHttpHeaders { ContentType = contentType };
                await blobClient.SetHttpHeadersAsync(httpHeaders);

                var blobUrl = blobClient.Uri.ToString();
                return blobUrl;
            }
            catch (Azure.RequestFailedException ex)
            {
                throw new InvalidOperationException($"Failed to upload blob '{blobName}': {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
