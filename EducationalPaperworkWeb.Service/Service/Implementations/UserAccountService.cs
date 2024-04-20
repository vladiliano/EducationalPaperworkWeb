using EducationalPaperworkWeb.Domain.Domain.Enums.In_Program_Enums;
using EducationalPaperworkWeb.Domain.Domain.Models.ResponseEntities;
using EducationalPaperworkWeb.Domain.Domain.Models.UserEntities;
using EducationalPaperworkWeb.Repository.Repository.Interfaces.UnitOfWork;
using EducationalPaperworkWeb.Service.Service.Helpers.Hashing;
using EducationalPaperworkWeb.Service.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;


namespace EducationalPaperworkWeb.Service.Service.Implementations
{
    public class UserAccountService : IUserAccountService
    {
        private readonly IUnitOfWork _repository;

		public UserAccountService(IUnitOfWork repository)
        {
            _repository = repository;
		}

        public async Task<IBaseResponse<bool>> ChangePasswordAsync(UserRestorePassword user)
        {
            try
            {
                if (user == null) throw new Exception($"{nameof(ChangePasswordAsync)}: {nameof(user)} == {user}");
                var existUser = await _repository.UserRepository.GetAll().FirstOrDefaultAsync(x => x.Email == user.Email);

                if (existUser == null)
                {
                    return new BaseResponse<bool>()
                    {
                        StatusCode = OperationStatusCode.NotFound,
                        Description = "Користувача з такою поштою не знайдено!",
                        Data = false
                    };
                }

                if(user.Password != user.RepeatPassword)
                {
                    return new BaseResponse<bool>()
                    {
                        StatusCode = OperationStatusCode.BadRequest,
                        Description = "Паролі не співпадають!",
                        Data = false
                    };
                }

                existUser.Password = SecurityUtility.HashPassword(user.Email + user.Password);
                await _repository.UserRepository.UpdateAsync(existUser);

                return new BaseResponse<bool>()
                {
                    Description = "Пароль успішно змінено!",
                    StatusCode = OperationStatusCode.OK,
                    Data = true
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>()
                {
                    Description = ex.Message,
                    StatusCode = OperationStatusCode.InternalServerError,
                    Data = false
                };
            }
        }

        public async Task<IBaseResponse<ClaimsIdentity>> SignInAsync(UserSignIn user)
        {
            try
            {
                if (user == null) throw new Exception($"{nameof(SignInAsync)}: {nameof(user)} == {user}");

                var existUser = await _repository.UserRepository.GetAll().FirstOrDefaultAsync(x => x.Email == user.Email);

                if (existUser == null)
                {
                    return new BaseResponse<ClaimsIdentity>()
                    {
                        Description = "Користувача з такою поштою не існує!",
                        StatusCode = OperationStatusCode.NotFound
                    };
                }

                if (existUser.Password != SecurityUtility.HashPassword(user.Email + user.Password))
                {
                    return new BaseResponse<ClaimsIdentity>()
                    {
                        Description = "Невірний пароль!",
                        StatusCode = OperationStatusCode.Unauthorized,
                    };
                }

                var result = Authenticate(existUser);

                return new BaseResponse<ClaimsIdentity>()
                {
                    Description = "Користувач успішно увійшов в аккаунт!",
                    StatusCode = OperationStatusCode.OK,
                    Data = result
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<ClaimsIdentity>()
                {
                    Description = ex.Message,
                    StatusCode = OperationStatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<User>> SignUpAsync(UserSignUp user)
        {
            try
            {
                if (user == null) throw new Exception($"{nameof(SignUpAsync)}: {nameof(user)} == {user}");

                var existUser = await _repository.UserRepository.GetAll().FirstOrDefaultAsync(x => x.Email == user.Email);

                if (existUser != null)
                {
                    return new BaseResponse<User>()
                    {
                        Description = "Користувач з такою поштою вже існує!",
                        StatusCode = OperationStatusCode.Conflict,
                        Data = user
                    };
                }

                user.Password = SecurityUtility.HashPassword(user.Email + user.Password);

                await _repository.UserRepository.CreateAsync(user);

                return new BaseResponse<User>()
                {
                    Description = "Користувача успішно зареєстровано!",
                    StatusCode = OperationStatusCode.Created,
                    Data = user
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<User>()
                {
                    Description = ex.Message,
                    StatusCode = OperationStatusCode.InternalServerError
                };
            }
        }

        private ClaimsIdentity Authenticate(User user)
        {
            var claims = new List<Claim>
            {
                new(ClaimsIdentity.DefaultNameClaimType, user.Id.ToString()),
                new(ClaimsIdentity.DefaultRoleClaimType, user.Role.ToString()),
            };

            return new ClaimsIdentity(claims, "ApplicationCookie");
		}
    }
}
