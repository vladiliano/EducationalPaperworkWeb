using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.StaticFiles;
using EducationalPaperworkWeb.Domain.Domain.Enums.In_Program_Enums;
using EducationalPaperworkWeb.Domain.Domain.Models.ResponseEntities;
using EducationalPaperworkWeb.Infrastructure.Infrastructure.DataStorage.Interface;
using EducationalPaperworkWeb.Domain.Domain.Models.In_Program;

namespace EducationalPaperworkWeb.Infrastructure.Infrastructure.DataStorage.BlobStorage
{
    public class BlobStorage : IDataStorage
    {
        private readonly IConfiguration _configuration;
        private readonly Task<string> _connectionStringTask;
        private readonly string _containerName;

        public BlobStorage(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionStringTask = GetConnectionStringAsync();
            _containerName = _configuration.GetConnectionString("BlobStorageContainerName");
        }

        private async Task<string> GetConnectionStringAsync()
        {
            var keyVaultUrl = _configuration.GetConnectionString("BlobStorageKeyVaultUrl");
            var keyVaultName = _configuration.GetConnectionString("BlobStorageConnectionStringName");
            var secretClient = new SecretClient(new Uri(keyVaultUrl), new DefaultAzureCredential());
            var connectionStringSecret = await secretClient.GetSecretAsync(keyVaultName);
            return connectionStringSecret.Value.Value;
        }

        public async Task<IBaseResponse<string>> UploadFileAsync(IFormFile file)
        {
            try
            {
                var connectionString = await _connectionStringTask;

                var fileName = $"{Guid.NewGuid()}*{file.FileName}";

                var container = new BlobContainerClient(connectionString, _containerName);
                var blobClient = container.GetBlobClient(fileName);

                using (var fileStream = file.OpenReadStream())
                {
                    await blobClient.UploadAsync(fileStream, true);
                }

                return new BaseResponse<string>
                {
                    Data = fileName,
                    StatusCode = OperationStatusCode.OK,
                    Description = $"{nameof(UploadFileAsync)}: Файл був успішно завантажений на сервер."
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<string>
                {
                    StatusCode = OperationStatusCode.InternalServerError,
                    Description = $"{nameof(UploadFileAsync)}: Файл не був завантажений на сервер. Виникла помилка - {ex.Message}"
                };
            }
        }

        public async Task<IBaseResponse<FileResponse>> GetFileAsync(string fileName)
        {
            try
            {
                var connectionString = await _connectionStringTask;

                var container = new BlobContainerClient(connectionString, _containerName);
                var blobClient = container.GetBlobClient(fileName);

                var contentTypeProvider = new FileExtensionContentTypeProvider();

                if (!contentTypeProvider.TryGetContentType(fileName, out var mimeType))
                {
                    mimeType = "application/octet-stream";
                }

                var displayFileName = fileName.Split('*')[1];
                displayFileName = displayFileName.Split('.')[0];

                using var memoryStream = new MemoryStream();
                await blobClient.DownloadToAsync(memoryStream);

                return new BaseResponse<FileResponse>
                {
                    Data = new FileResponse
                    {
                        Content = memoryStream.ToArray(),
                        Mime = mimeType,
                        Name = displayFileName
                    },
                    StatusCode = OperationStatusCode.OK,
                    Description = $"{nameof(GetFileAsync)}: Файл был успешно получен с сервера."
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<FileResponse>
                {
                    StatusCode = OperationStatusCode.InternalServerError,
                    Description = $"{nameof(GetFileAsync)}: Файл не был получен с сервера. Произошла ошибка - {ex.Message}"
                };
            }
        }
    }
}
