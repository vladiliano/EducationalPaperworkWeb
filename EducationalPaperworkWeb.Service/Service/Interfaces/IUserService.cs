using EducationalPaperworkWeb.Domain.Domain.Models.ResponseEntities;
using EducationalPaperworkWeb.Domain.Domain.Models.UserEntities;
using Microsoft.AspNetCore.Http;

namespace EducationalPaperworkWeb.Service.Service.Interfaces
{
    public interface IUserService
    {
        public Task<IBaseResponse<User>> GetUserAsync(long id);
        public IBaseResponse<long> GetUserId(HttpContext context);
    }
}
