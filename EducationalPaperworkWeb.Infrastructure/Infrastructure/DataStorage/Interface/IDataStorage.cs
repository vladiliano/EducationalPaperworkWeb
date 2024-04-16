using EducationalPaperworkWeb.Domain.Domain.Models.In_Program;
using EducationalPaperworkWeb.Domain.Domain.Models.ResponseEntities;
using Microsoft.AspNetCore.Http;

namespace EducationalPaperworkWeb.Infrastructure.Infrastructure.DataStorage.Interface
{
    public interface IDataStorage
    {
        public Task<IBaseResponse<string>> UploadFileAsync(IFormFile file);
        public Task<IBaseResponse<FileResponse>> GetFileAsync(string fileName);
    }
}
