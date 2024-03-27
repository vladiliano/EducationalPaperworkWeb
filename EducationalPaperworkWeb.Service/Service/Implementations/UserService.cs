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
        public IBaseResponse<long> GetUserId(HttpContext context)
        {
            try
            {
                if (!long.TryParse(context.User.FindFirst("UserId")?.Value, out long userId))
                    throw new Exception("Помилка при спробі отримати Id користувача!");

                return new BaseResponse<long>()
                {
                    Description = "Id користувача успішно отримано!",
                    StatusCode = OperationStatusCode.OK,
                    Data = userId
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<long>()
                {
                    Description = nameof(GetUserId) + ": " + ex.Message,
                    StatusCode = OperationStatusCode.InternalServerError,
                };
            }
        }
    }
}
