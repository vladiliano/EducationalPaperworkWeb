using EducationalPaperworkWeb.Domain.Domain.Enums.In_Program_Enums;
using EducationalPaperworkWeb.Domain.Domain.Models.ResponseEntities;
using EducationalPaperworkWeb.Domain.Domain.Models.UserEntities;
using EducationalPaperworkWeb.Repository.Repository.Interfaces.UnitOfWork;
using EducationalPaperworkWeb.Service.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace EducationalPaperworkWeb.Service.Service.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _repository;

        public UserService(IUnitOfWork repository)
        {
            _repository = repository;
        }
        public async Task<IBaseResponse<User>> GetUserAsync(long id)
        {
            try
            {
                var user = await _repository.UserRepository.GetAll().FirstOrDefaultAsync(x => x.Id == id);

                if (user == null)
                {
                    return new BaseResponse<User>()
                    {
                        Description = "Користувача з таким id не знайдено!",
                        StatusCode = OperationStatusCode.NotFound
                    };
                }

                return new BaseResponse<User>()
                {
                    Description = "Користувача знайдено!",
                    StatusCode = OperationStatusCode.OK,
                    Data = user
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<User>()
                {
                    Description = nameof(GetUserAsync) + ": " + ex.Message,
                    StatusCode = OperationStatusCode.InternalServerError
                };
            }
        }
        public IBaseResponse<string> GetUserCookieField(HttpContext context, string value)
        {
            try
            {
                var field = (context.User.FindFirst(value)?.Value)
                    ?? throw new Exception($"Помилка при спробі отримати дані користувача з cookie за ключем {value}.");

                return new BaseResponse<string>()
                {
                    Description = $"Дані користувача з cookie за ключем {value} успішно отримано!",
                    StatusCode = OperationStatusCode.OK,
                    Data = field
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<string>()
                {
                    Description = nameof(GetUserCookieField) + ": " + ex.Message,
                    StatusCode = OperationStatusCode.InternalServerError,
                };
            }
        }
    }
}
