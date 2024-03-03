using EducationalPaperworkWeb.Domain.Domain.Enums.In_Program_Enums;
using EducationalPaperworkWeb.Domain.Domain.Models.Response;
using EducationalPaperworkWeb.Domain.Domain.Models.User;
using EducationalPaperworkWeb.Infrastructure.Infrastructure.Helpers.PasswordHashing;
using EducationalPaperworkWeb.Repository.Repositories.Implementations;
using EducationalPaperworkWeb.Repository.Repositories.Interfaces;
using EducationalPaperworkWeb.Service.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EducationalPaperworkWeb.Service.Service.Implementations
{
    public class UserAccountService : IUserAccountService
    {
        private readonly IBaseRepository<User> _repository;

        public UserAccountService(IBaseRepository<User> repository)
        {
            _repository = repository;
        }

        public async Task<IBaseResponse<bool>> ChangePassword(UserChangePassword user)
        {
            try
            {
                if (user == null) throw new Exception($"{nameof(Register)}: user == null");

                var existUser = await _repository.GetAll().FirstOrDefaultAsync(x => x.Email == user.Email);

                if (existUser == null)
                {
                    return new BaseResponse<bool>()
                    {
                        StatusCode = StatusCode.NotFound,
                        Description = "Користувача з такою поштою не знайдено!",
                        Data = false
                    };
                }

                if(user.Password != user.RepeatPassword)
                {
                    return new BaseResponse<bool>()
                    {
                        StatusCode = StatusCode.BadRequest,
                        Description = "Паролі не співпадають!",
                        Data = false
                    };
                }

                existUser.Password = PasswordHasher.HashPassowrd(user.Password);
                await _repository.Update(existUser);

                return new BaseResponse<bool>()
                {
                    Description = "Пароль успішно змінено!",
                    StatusCode = StatusCode.OK,
                    Data = true
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError,
                    Data = false
                };
            }
        }

        public async Task<IBaseResponse<ClaimsIdentity>> LogIn(UserLogIn user)
        {
            try
            {
                if (user == null) throw new Exception($"{nameof(Register)}: user == null");

                var existUser = await _repository.GetAll().FirstOrDefaultAsync(x => x.Email == user.Email);

                if (existUser == null)
                {
                    return new BaseResponse<ClaimsIdentity>()
                    {
                        Description = "Користувача з такою поштою не знайдено!",
                        StatusCode = StatusCode.NotFound
                    };
                }

                if (existUser.Password != PasswordHasher.HashPassowrd(user.Password))
                {
                    return new BaseResponse<ClaimsIdentity>()
                    {
                        Description = "Невірний пароль!",
                        StatusCode = StatusCode.Unauthorized,
                    };
                }

                var result = Authenticate(existUser);

                return new BaseResponse<ClaimsIdentity>()
                {
                    Description = "Користувач успішно увійшов в аккаунт!",
                    StatusCode = StatusCode.OK,
                    Data = result
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<ClaimsIdentity>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<User>> Register(User user)
        {
            try
            {
                if (user == null) throw new Exception($"{nameof(Register)}: user == null");

                var existUser = await _repository.GetAll().FirstOrDefaultAsync(x => x.Email == user.Email);

                if (existUser != null)
                {
                    return new BaseResponse<User>()
                    {
                        Description = "Користувач з такою поштою вже існує!",
                        StatusCode = StatusCode.Conflict,
                        Data = user
                    };
                }

                user.Password = PasswordHasher.HashPassowrd(user.Password);

                await _repository.Create(user);

                return new BaseResponse<User>()
                {
                    Description = "Користувача успішно зареєстровано!",
                    StatusCode = StatusCode.Created,
                    Data = user
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<User>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        private ClaimsIdentity Authenticate(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Name),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.ToString())
            };
            return new ClaimsIdentity(claims, "ApplicationCookie",
                ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
        }
    }
}
